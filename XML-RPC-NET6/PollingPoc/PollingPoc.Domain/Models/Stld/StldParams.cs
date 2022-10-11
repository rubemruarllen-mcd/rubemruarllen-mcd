namespace PollingPoc.Domain.Models.Stld
{
    public class StldParams
    {
        public string? DataType { get; set; }
        public string? PosList { get; set; }
        public string? CheckPoint { get; set; }
        public string? PIIMaskType { get; set; }
        public string? CypherKey { get; set; }
        public string? PIIMaskIdType { get; set; }
        public string? RecordId { get; set; }

        public string? MaxEvents { get; set; }
    }
}
