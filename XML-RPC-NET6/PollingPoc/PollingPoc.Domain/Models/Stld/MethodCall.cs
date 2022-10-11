using System.Xml;
using System.Xml.Serialization;

namespace PollingPoc.Domain.Models.Stld
{
    [XmlRoot(ElementName = "methodCall")]
    public class MethodCall
    {

        [XmlElement(ElementName = "methodName")]
        public string? MethodName { get; set; }

        [XmlElement(ElementName = "params")]
        public Params? Params { get; set; }

        public void GenerateMethodCallStld(StldParams methodCallStld, DateOnly businessDate)
        {
            MethodName = "datarequest";
            Params = new Params();
            Params.Param = new List<Param>();
            var paramDataType = new Param() { Value = new Value() { String = methodCallStld.DataType },XmlComment = new XmlDocument().CreateComment("dataType") };
            var paramBusinessDate = new Param() { Value = new Value() { String = businessDate.ToString("yyyyMMdd") },XmlComment = new XmlDocument().CreateComment("businessDate") };
            var paramPosList = new Param() { Value = new Value() { String = methodCallStld.PosList } , XmlComment = new XmlDocument().CreateComment("posList")};
            var paramCheckPoint = new Param() { Value = new Value() { String = methodCallStld.CheckPoint }, XmlComment = new XmlDocument().CreateComment("checkPoint") };
            var paramPIIMaskType = new Param() { Value = new Value() { String = methodCallStld.PIIMaskType }, XmlComment = new XmlDocument().CreateComment("PIImaskType") };
            var paramCypherKey = new Param() { Value = new Value() { String = methodCallStld.CypherKey } , XmlComment = new XmlDocument().CreateComment("cypherKey")};
            var paramPIIMaskIdType = new Param() { Value = new Value() { String = methodCallStld.PIIMaskIdType }, XmlComment = new XmlDocument().CreateComment("PIImaskType") };
            var paramRecordId = new Param() { Value = new Value() { String = "" }, XmlComment = new XmlDocument().CreateComment("recordId")};
            var maxEvents = new Param() { Value = new Value() { String = methodCallStld.MaxEvents } , XmlComment = new XmlDocument().CreateComment ("MaxEvents")};
            Params.Param.Add(paramDataType);
            Params.Param.Add(paramBusinessDate);
            Params.Param.Add(paramPosList);
            Params.Param.Add(paramCheckPoint);
            Params.Param.Add(paramPIIMaskType);
            Params.Param.Add(paramCypherKey);
            Params.Param.Add(paramPIIMaskIdType);
            Params.Param.Add(paramRecordId);
            Params.Param.Add(maxEvents);
        }
    }
}
