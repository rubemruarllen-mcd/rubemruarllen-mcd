using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "Item")]
    public class Item
    {
        [XmlAttribute(AttributeName = "code")]
        public string? Code { get; set; }
        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; set; }
        [XmlAttribute(AttributeName = "qty")]
        public string? Qty { get; set; }
        [XmlAttribute(AttributeName = "totalPrice")]
        public string? TotalPrice { get; set; }
        [XmlAttribute(AttributeName = "totalTax")]
        public string? TotalTax { get; set; }
        [XmlAttribute(AttributeName = "dayPart")]
        public string? DayPart { get; set; }
    }
}
