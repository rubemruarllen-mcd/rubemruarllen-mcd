using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "params")]
    public class Params
    {
        [XmlElement(ElementName = "param")]
        public List<Param>? Param { get; set; }
    }
}
