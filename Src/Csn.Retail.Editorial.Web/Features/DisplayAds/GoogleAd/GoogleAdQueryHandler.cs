using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd
{
    [AutoBind]
    public class GoogleAdQueryHandler : IQueryHandler<DisplayAdQuery, GoogleAdViewModel>
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public GoogleAdQueryHandler(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }
        public GoogleAdViewModel Handle(DisplayAdQuery displayAdQuery)
        {
            if (!GoogleAdSettings.GoogleAdTypes.TryGetValue(displayAdQuery.AdPlacement, out var adSetting))
            {
                return null;
            }

            return new GoogleAdViewModel()
            {
                Description = displayAdQuery.AdPlacement.ToString(),
                Dimensions = JsonConvert.SerializeObject(adSetting.DisplayAdSize.Dimensions()),
                AdNetworkCode = _tenantProvider.Current().GoogleAdsNetworkCode,
                AdUnitId = adSetting.UnitId,
                AdSlotId = adSetting.SlotId
            };
        }
    }
}