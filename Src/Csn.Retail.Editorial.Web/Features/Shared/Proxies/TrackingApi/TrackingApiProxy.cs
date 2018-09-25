using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
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
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public TrackingApiProxy(ISmartServiceClient smartClient, ITenantProvider<TenantInfo> tenantProvider)
        {
            _smartClient = smartClient;
            _tenantProvider = tenantProvider;
        }

        public Task<SmartServiceResponse<TrackingApiDto>> GetTracking(TrackingApiInput input)
        {
            var client = _smartClient.Service(ServiceName)
                .Path("v1/api/tracking/script")
                .QueryString("Application", input.ApplicationName);

            if (_tenantProvider.Current().IncludeNielsen)
            {
                client.QueryString("IncludeNielsen", "true");
            }
                
            return client.GetAsync<TrackingApiDto>();
        }
    }
}