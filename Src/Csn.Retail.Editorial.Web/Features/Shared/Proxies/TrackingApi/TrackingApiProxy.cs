using System.Threading.Tasks;
using Csn.Hystrix.RestClient;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi
{
    public interface ITrackingApiProxy
    {
        Task<TrackingApiDto> GetTracking(TrackingApiInput input);
    }

    [AutoBind]
    public class TrackingApiProxy : ITrackingApiProxy
    {
        private const string HostName = "TrackingScriptApiProxy";
        private readonly IFluentHystrixRestClientFactory _restClient;

        public TrackingApiProxy(IFluentHystrixRestClientFactory restClient)
        {
            _restClient = restClient;
        }

        public async Task<TrackingApiDto> GetTracking(TrackingApiInput input)
        {
            var response = await _restClient.HostName(HostName)
                .Path("v1/api/tracking/script")
                .QueryParams("Application", input.ApplicationName)
                .GetAsync<TrackingApiDto>();

            return response.Result;
        }
    }
}