using PollingPoc.Domain.Interfaces;
using PollingPoc.Domain.Models.Stld;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using PollingPoc.Domain.Converters;
using PollingPoc.Services.Api;
using System.Security.AccessControl;
using System.Xml;

namespace PollingPoc.Services
{
    public class XmlRPC : IXmlRPC
    {
        private const string ERROR_NAME = "faultCode";
        private string ERROR_CODE = "99";
        private readonly IXmlRPCApi _xmlRPCApi;
        private readonly ILogger<IXmlRPC> _logger;
 

        public XmlRPC(IXmlRPCApi xmlRPCApi, ILogger<IXmlRPC> logger)
        {
            _xmlRPCApi = xmlRPCApi;
            _logger = logger;

        }

        /// <inheritdoc cref="IXmlRPC.GetSTLDReport"/>
        public async Task<MethodResponse?> RequestRPC(MethodCall methodCall)
        {
            var xmlString = methodCall.Serialize();
            string returnValue = "";
            var response = await _xmlRPCApi.RequestData(xmlString);
            try
            {
                if (!response!.IsSuccessStatusCode)
                    throw response.Error ?? new Exception();
                returnValue = response.Content != null ? response.Content : "";
                if (returnValue.Contains("base64"))
                    returnValue = returnValue.ParseXmlBase64ToString();
                return returnValue.ParseXML<MethodResponse>();
            }
            catch (Exception ex)
            {
                _logger.LogError("{msg} - {stack}", ex.Message, ex.StackTrace);
                _logger.LogInformation("XML RETURNED:{xml}",returnValue);
                return null;
            }
        }

        public async Task<string?> RequestXMLRPC(MethodCall methodCall)
        {
            var xmlString = methodCall.Serialize();
            string returnValue = "";
            var response = await _xmlRPCApi.RequestData(xmlString);
            try
            {
                if (!response!.IsSuccessStatusCode)
                    throw response.Error ?? new Exception();
                returnValue = response.Content != null ? response.Content : "";
                if (returnValue.Contains("base64"))
                    returnValue = returnValue.ParseXmlBase64ToString();
                return returnValue;
            }
            catch (Exception ex)
            {
                _logger.LogError("{msg} - {stack}", ex.Message, ex.StackTrace);
                _logger.LogInformation("XML RETURNED:{xml}", returnValue);
                return null;
            }
        }

        /// <inheritdoc cref="IXmlRPC.GetSTLDReport"/>
        public async Task<XmlDocument?> GetSTLDReportXml(DateOnly startDate, StldParams stldParams)
        {
            _logger.LogInformation("Getting STLD report");
            var id = await GetJobId(startDate, stldParams);

            if (id == null)
                return null;

            var methodCallQuery = CreateMethodCallQuery(id);
            string? response = null;
            XmlDocument xmlDoc = new XmlDocument();
            
            var underProcessing = true;

            while (underProcessing)
            {
                response = await RequestXMLRPC(methodCallQuery);
                xmlDoc.LoadXml(response);
                if (response == null)
                    break;
               var nodes = xmlDoc.SelectNodes("methodResponse/params/param/value/struct/member");
               foreach (var node in nodes)
               {
                   var xmlNode = (XmlNode)node;
                   underProcessing = xmlNode.InnerText == ERROR_NAME + ERROR_CODE;
                   break;
               }
                 await Task.Delay(1000);
            }

            _logger.LogInformation("STLD was got");
            return xmlDoc;
        }


        /// <inheritdoc cref="IXmlRPC.GetSTLDReport"/>
        public async Task<MethodResponse?> GetSTLDReport(DateOnly startDate, StldParams stldParams)
        {
            _logger.LogInformation("Getting STLD report");
            var id = await GetJobId(startDate,stldParams);

            if (id == null)
                return null;

            var methodCallQuery = CreateMethodCallQuery(id);
            MethodResponse? methodResponse = null;
            var underProcessing = true;

            while (underProcessing)
            {
                methodResponse = await RequestRPC(methodCallQuery);
                if (methodResponse == null)
                    break;
                underProcessing = methodResponse.Params!.Param![0].Value!.Struct!.Member!
                    .Any(member => member.Name == ERROR_NAME && member!.Value!.I4 == ERROR_CODE);
                await Task.Delay(1000);
            }

            _logger.LogInformation("STLD was got");
            return methodResponse;
        }

        /// <summary>
        /// This method do a request to get the JobId.
        /// </summary>
        /// <returns></returns>
        private async Task<string?> GetJobId(DateOnly startDate, StldParams stldParams)
        {
            const string MEMBER_NAME = "id";
            MethodCall methodCall = new MethodCall();
            methodCall.GenerateMethodCallStld(stldParams, startDate);
            var result = await RequestRPC(methodCall);

            var memberId = result?.Params.Param[0].Value.Struct.Member.FirstOrDefault(member => member.Name == MEMBER_NAME);
            var jobId = memberId?.Value.I4;

            _logger.LogInformation("STLD jobId: {0}", jobId);
            return jobId;
        }


        /// <summary>
        /// This method create a MethodCall model to use to call query.
        /// </summary>
        /// <param name="JobId"> This parameter is the JobId of query</param>
        /// <returns></returns>
        private MethodCall CreateMethodCallQuery(string JobId)
        {

            var methodCall = new MethodCall();
            methodCall.MethodName = "Query";
            methodCall.Params = new Params();
            methodCall.Params.Param = new List<Param>();
            var param = new Param() { Value = new Value() { I4 = JobId } };
            methodCall.Params.Param.Add(param);

            return methodCall;
        }
    }
}