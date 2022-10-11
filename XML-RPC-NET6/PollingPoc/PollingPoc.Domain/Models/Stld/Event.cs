using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "Event")]
    public class Event
    {
        [XmlAttribute(AttributeName = "RegId")]
        public string? RegId { get; set; }
        [XmlAttribute(AttributeName = "Type")]
        public string? Type { get; set; }
        [XmlAttribute(AttributeName = "Time")]
        public string? Time { get; set; }
        [XmlElement(ElementName = "TRX_Sale")]
        public TRX_Sale? TRX_Sale { get; set; }

        public Order? Order
        {
            get
            {
                return this.TRX_Sale?.Order;
            }
        }
    }
}
