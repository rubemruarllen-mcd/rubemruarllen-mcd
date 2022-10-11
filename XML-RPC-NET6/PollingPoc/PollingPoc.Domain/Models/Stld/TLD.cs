using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "TLD")]
    public class TLD
    {
        [XmlElement(ElementName = "Node")]
        public List<Node>? Node { get; set; }
        [XmlAttribute(AttributeName = "storeId")]
        public string? StoreId { get; set; }
        [XmlAttribute(AttributeName = "businessDate")]
        public string? BusinessDate { get; set; }

    }
}
