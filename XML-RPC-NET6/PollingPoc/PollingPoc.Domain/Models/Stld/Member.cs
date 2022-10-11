using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "member")]
    public class Member
    {
        [XmlElement(ElementName = "name")]
        public string? Name { get; set; }
        [XmlElement(ElementName = "value")]
        public Value? Value { get; set; }
    }
}
