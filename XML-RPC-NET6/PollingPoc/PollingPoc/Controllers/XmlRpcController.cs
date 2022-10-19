using Microsoft.AspNetCore.Mvc;
using PollingPoc.Domain.Interfaces;
using PollingPoc.Domain.Models.Stld;
using Refit;
using System.ComponentModel.DataAnnotations;
using System.Xml;
using PollingPoc.Domain.Converters;
using Swashbuckle.AspNetCore.Filters;
using PollingPoc.Swagger.Examples;
using PollingPoc.Services;
using PollingPoc.Domain.Models;

namespace PollingPoc.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public class XmlRpcController : ControllerBase
    {


        private readonly IXmlRPC _xmlRpc;
        private readonly IMessageBrokerFactory _messageBrokerFactory;


        public XmlRpcController(IXmlRPC xmlRpc, IMessageBrokerFactory messageBrokerFactory)
        {
            _xmlRpc = xmlRpc;
            _messageBrokerFactory = messageBrokerFactory;
        }

        private static readonly StldParams _stldParams = new StldParams()
        {
            CheckPoint = "0",
            CypherKey = "",
            DataType = "STLD",
            PIIMaskIdType = "0",
            PIIMaskType = "0",
            PosList = "",
            RecordId = ""
        };

        //[HttpPost(Name = "FullReport")]
        //public ActionResult<MethodResponse> Report([FromBody] StldParams stldParams, [Required][FromHeader] DateTime BusinessDay , [FromHeader] bool useDefaultParams = true)
        //{
        //    //var result = new MethodResponse();
        //    var parameters = stldParams;
        //    if (useDefaultParams)
        //        parameters = _stldParams;


        //   var result = _xmlRpc.GetSTLDReport(DateOnly.FromDateTime(BusinessDay), parameters);


        //    return result.Result;
        //}

        [HttpPost]
        [Route("StldReport")]
        [SwaggerRequestExample(typeof(StldParams), typeof(StldParamsRequestExample))]
        public async Task<ActionResult<string>> StldReportX([Required][FromHeader] DateTime businessDay, [FromBody] StldParams stldParams)
        {

            var result = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(businessDay), stldParams);
            var error = new { CheckPoint = "" };
            var r = new[] { error }.ToList();
            r.Remove(error);

            var xml = result.XmlToString(1);

            _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(message: xml);



            var tld = result.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");

            if (tld == null)
                return "Stld is null";

            var hasMoreContent =
               tld.Attributes["hasMoreContent"]!.Value == "true";
            stldParams.CheckPoint = tld.Attributes["checkPoint"]!.Value;
           
            while (hasMoreContent)
            {
                try
                {
                    var update = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(businessDay), stldParams);
                    var newPeaceStld =
                        update.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");
                    if (Int32.Parse(stldParams.CheckPoint) > 2500)
                    {
                        var ok = 0;
                    }
                    hasMoreContent = newPeaceStld.Attributes["hasMoreContent"]!.Value == "true";
                    stldParams.CheckPoint = newPeaceStld.Attributes["checkPoint"]!.Value;

                    _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(subject: "PocReport", message: "A0CHECKPOINT:" + stldParams.CheckPoint);
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
                            _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(subject: "PocReport", message: up.XmlToString(1));
                        }

                    }
                }
                catch (Exception exception)
                {
                    r.Add(new { CheckPoint = stldParams.CheckPoint });
                    stldParams.CheckPoint = (Int32.Parse(stldParams.CheckPoint) + 10).ToString();
                    if (r.Count() > 20)
                    {
                        break;
                    }
                    _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(subject: "PocReport", message: "ERROR-TLD-CHECKPOINT:"+ stldParams.CheckPoint);
                }
            }



            return tld.XmlToString(1);
        }


        [HttpGet]
        [Route("StldReportReal")]
        [SwaggerRequestExample(typeof(StldParams), typeof(StldParamsRequestExample))]
        public async Task<ActionResult<object>> StldReportReal()
        {
            var businessDay = DateTime.Now;
            _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(message: "G");
            _stldParams.MaxEvents = "10";
            var result = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(businessDay), _stldParams);
            var error = new { CheckPoint = "" };
            var r = new[] { error }.ToList();
            r.Remove(error);
            var alredyNotified = false;

            var tld = result.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");

            var hasMoreContent =
               tld.Attributes["hasMoreContent"]!.Value == "true";
            _stldParams.CheckPoint = tld.Attributes["checkPoint"]!.Value;

            while (true)
            {
                try
                {
                    var update = await _xmlRpc.GetSTLDReportXml(DateOnly.FromDateTime(DateTime.Today), _stldParams);
                    var newPeaceStld =
                        update.SelectSingleNode("methodResponse/params/param/value/struct/member/value/base64/TLD");
                    hasMoreContent = newPeaceStld.Attributes["hasMoreContent"]!.Value == "true";
                    if (!hasMoreContent)
                    {
                        _stldParams.MaxEvents = "1";
                        if (!alredyNotified)
                        {
                            _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(subject: "PocReport", message: "UPDATED-A0");
                            alredyNotified = true;
                        }
                    }
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
                                _messageBrokerFactory.GetCommunicationServices(CommunicationService.NatsService).Publish(subject: "PocReport", message: sigleEvent.XmlToString(1));
                                up.AppendChild(importNode);
                            }
                        }

                    }
                }
                catch (Exception exception)
                {
                    r.Add(new { CheckPoint = _stldParams.CheckPoint });
                    _stldParams.CheckPoint = (Int32.Parse(_stldParams.CheckPoint) + 10).ToString();
                }
            }

            return new { Errors = r, TLD = tld.XmlToString(1) };
        }
    }
}
