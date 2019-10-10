using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.ServiceClient.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi
{
    public interface ITrackingApiProxy
    {
        Task<SmartServiceResponse<TrackingApiDto>> GetTracking(TrackingApiInput input);
    }

    [AutoBind]
    public class TrackingApiProxy : ITrackingApiProxy
    {
        private const string ServiceName = "api-tracking-ga-scripts";
        private readonly ISmartServiceClient _smartClient;

        public TrackingApiProxy(ISmartServiceClient smartClient)
        {
            _smartClient = smartClient;
        }

        public Task<SmartServiceResponse<TrackingApiDto>> GetTracking(TrackingApiInput input)
        {
            var client = _smartClient.Service(ServiceName)
                .Path("v1/api/tracking/script")
                .QueryString("Application", input.ApplicationName)
                .QueryString("includebitracking", "true")
                .QueryString("includegooglesem", input.IncludeGoogleSem.ToString())
                .QueryString("ClientGeoCountryCode", input.ClientGeoCountryCode);

            if (input.IncludeNielsen)
            {
                client.QueryString("IncludeNielsen", "true");
            }
                
            return client.GetAsync<TrackingApiDto>();
        }
    }
}