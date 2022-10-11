using Refit;

namespace PollingPoc.Services.Api
{
    public interface IXmlRPCApi
    {
        [Post("/goform/RPC2")]
        Task<IApiResponse<string>> RequestData([Body] string methodCall);
    }
}
