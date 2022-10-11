using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "TRX_Sale")]
    public class TRX_Sale
    {
        [XmlElement(ElementName = "Order")]
        public Order? Order { get; set; }
        [XmlAttribute(AttributeName = "status")]
        public string? Status { get; set; }
        [XmlAttribute(AttributeName = "POD")]
        public string? POD { get; set; }
    }
}
