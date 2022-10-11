using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "Node")]
    public class Node
    {
        [XmlAttribute(AttributeName = "id")]
        public string? Id { get; set; }
        [XmlAttribute(AttributeName = "nodeStatus")]
        public string? NodeStatus { get; set; }

        [XmlElement(ElementName = "Event")]
        public List<Event>? Event { get; set; }

        [XmlIgnore]
        public IEnumerable<Event>? SalesEvents
        {
            get
            {
                return Event?.Where(p => p.Type == "TRX_Sale");
            }
        }
    }
}
