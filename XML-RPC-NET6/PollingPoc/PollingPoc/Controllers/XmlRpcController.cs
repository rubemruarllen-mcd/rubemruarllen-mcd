using Microsoft.AspNetCore.Mvc;
using PollingPoc.Domain.Interfaces;
using PollingPoc.Domain.Models.Stld;
using Refit;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using PollingPoc.Domain.Converters;

namespace PollingPoc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class XmlRpcController : Controller
    {


        private readonly IXmlRPC _xmlRpc;

        public XmlRpcController(IXmlRPC xmlRpc)
        {
            _xmlRpc = xmlRpc;
        }

        private static readonly StldParams _stldParams = new StldParams()
        {
            CheckPoint = "",
            CypherKey = "",
            DataType = "STLD",
            PIIMaskIdType = "0",
            PIIMaskType = "0",
            PosList = "",
            RecordId = ""
        };




        [HttpPost(Name = "Report")]
        public ActionResult<MethodResponse> Report([FromBody] StldParams stldParams, [Required][FromHeader] DateTime BusinessDay , [FromHeader] bool useDefaultParams = true)
        {
            //var result = new MethodResponse();
            var parameters = stldParams;
            if (useDefaultParams)
                parameters = _stldParams;


           var result = _xmlRpc.GetSTLDReport(DateOnly.FromDateTime(BusinessDay), parameters);


            return result.Result;
        }

        [HttpGet(Name = "GetStldReport")]
        public async Task<ActionResult<object>> StldReport( [FromHeader] DateTime BusinessDay)
        {
            _stldParams.MaxEvents = "10";
            _stldParams.CheckPoint = "0";
            var result = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(DateTime.Today), _stldParams);
            var error = new { CheckPoint = "" };
            var r = new[] { error }.ToList();
            r.Remove(error);

            var tld = result.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");

            var hasMoreContent =
               tld.Attributes["hasMoreContent"]!.Value == "true";
            _stldParams.CheckPoint = tld.Attributes["checkPoint"]!.Value;

            while (hasMoreContent)
            {


                try
                {
                    var update = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(DateTime.Today), _stldParams);
                    var newPeaceStld =
                        update.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");
                    hasMoreContent = newPeaceStld.Attributes["hasMoreContent"]!.Value == "true";
                    _stldParams.CheckPoint = newPeaceStld.Attributes["checkPoint"]!.Value;
                    var nodes = newPeaceStld.SelectNodes("Node");
                    foreach (var node in nodes)
                    {
                        var xmlNode = (XmlNode)node;
                        var nodeId = xmlNode.Attributes["id"].Value;
                        var events = xmlNode.SelectNodes("Event");
                        if (events.Count > 0)
                        {
                            var up = tld.SelectSingleNode($"Node[@id='{nodeId}']");
                            foreach (var eventX in events)
                            {
                                var sigleEvent = (XmlNode)eventX;
                                //necessary for crossing XmlDocument contexts
                                var importNode = up.OwnerDocument.ImportNode(sigleEvent, true);
                                up.AppendChild(importNode);
                            }
                        }

                    }
                }
                catch (Exception exception)
                {
                    r.Add(new{ CheckPoint = _stldParams.CheckPoint});
                    _stldParams.CheckPoint = (Int32.Parse(_stldParams.CheckPoint) +10).ToString();
                }
            }


            return new { Errors = r, TLD = tld.XmlToString(1) };
        }
    }
}
