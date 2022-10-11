using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "base64")]
    public class Base64
    {
        [XmlElement(ElementName = "TLD")]
        public TLD? TLD { get; set; }
    }
}
