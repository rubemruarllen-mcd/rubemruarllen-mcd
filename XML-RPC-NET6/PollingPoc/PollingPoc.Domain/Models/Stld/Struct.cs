using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "struct")]
    public class Struct
    {
        [XmlElement(ElementName = "member")]
        public List<Member>? Member { get; set; }
    }
}
