using PollingPoc.Domain.Models.Stld;
using Swashbuckle.AspNetCore.Filters;

namespace PollingPoc.Swagger.Examples
{
    public class StldParamsRequestExample : IExamplesProvider<StldParams>
    {
        public StldParams GetExamples()
        {
            return new StldParams()
            {
                CheckPoint = "0",
                CypherKey = "",
                DataType = "STLD",
                PIIMaskIdType = "0",
                PIIMaskType = "0",
                PosList = "",
                RecordId = "",
                MaxEvents = "10"
            };
        }
    }
}
