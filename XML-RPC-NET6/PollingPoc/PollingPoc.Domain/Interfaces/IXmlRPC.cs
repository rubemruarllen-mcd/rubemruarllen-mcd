using PollingPoc.Domain.Models.Stld;
using System.Xml;

namespace PollingPoc.Domain.Interfaces
{
    public interface IXmlRPC
    {
        /// <summary>
        /// This method makes a request RPC.
        /// </summary>
        /// <param name="methodCall">This paramenter is the MethodCall to request</param>
        /// <returns>returns a MethodResponse</returns>
        Task<MethodResponse?> RequestRPC(MethodCall methodCall);

        /// <summary>
        /// This method gets the stld report.
        /// </summary>
        /// <returns>returns the report</returns>
        Task<MethodResponse?> GetSTLDReport(DateOnly startDate, StldParams stldParams);

        Task<XmlDocument?> GetSTLDReportXml(DateOnly startDate, StldParams stldParams);
    }
}
