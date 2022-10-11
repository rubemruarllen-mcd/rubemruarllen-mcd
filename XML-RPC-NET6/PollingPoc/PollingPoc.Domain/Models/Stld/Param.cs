using System.Xml;
using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "param")]
    public class Param
    {
        [XmlElement(ElementName = "value")]
        public Value? Value { get; set; }

        [XmlAnyElement("XmlComment")]
        public XmlComment XmlComment { get; set; }
    }
}
