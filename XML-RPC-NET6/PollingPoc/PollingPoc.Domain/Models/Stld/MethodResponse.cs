using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "methodResponse")]
    public class MethodResponse
    {
        [XmlElement(ElementName = "params")]
        public Params? Params { get; set; }

        [XmlIgnore]
        public List<Node>? Nodes
        {
            get
            {
                return Params?.Param?[0].Value?.Struct?.Member?[1].Value?.Base64?.TLD?.Node;
            }
        }
    }
}
