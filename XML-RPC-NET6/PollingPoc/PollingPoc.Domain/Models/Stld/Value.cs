using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "value")]
    public class Value
    {
        [XmlElement(ElementName = "i4")]
        public string? I4 { get; set; }
        [XmlElement(ElementName = "base64")]
        public Base64? Base64 { get; set; }

        [XmlElement(ElementName = "string")]
        public string? String { get; set; }

        [XmlElement(ElementName = "struct")]
        public Struct? Struct { get; set; }
    }
}
