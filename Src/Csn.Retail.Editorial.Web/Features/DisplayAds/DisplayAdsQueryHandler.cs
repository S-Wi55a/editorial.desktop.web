using System.Collections.Generic;
using System.Linq;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    [AutoBind]
    public class DisplayAdsQueryHandler : IQueryHandler<DisplayAdsQuery, IDisplayAdsModel>
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IEnumerable<IMediaMotiveTagBuilder> _tagBuilders;

        public DisplayAdsQueryHandler(IEnumerable<IMediaMotiveTagBuilder> tagBuilders, ITenantProvider<TenantInfo> tenantProvider)
        {
            _tagBuilders = tagBuilders;
            _tenantProvider = tenantProvider;
        }
        public IDisplayAdsModel Handle(DisplayAdsQuery displayAdsQuery)
        {
            if (_tenantProvider.Current().HasMediaMotive)
            {
                return GetMediaMotiveModel(displayAdsQuery);
            }

            return _tenantProvider.Current().HasGoogleAds ? GetGoogleAdsModel(displayAdsQuery) : null;
        }

        private IDisplayAdsModel GetMediaMotiveModel(DisplayAdsQuery displayAdsQuery)
        {
            // lookup the ad settings for this type
            if (!DisplayAdsSettings.MediaMotiveAdTypes.TryGetValue(displayAdsQuery.AdType, out var adSetting))
            {
                return null;
            }

            var mediaMotiveTagBuildersParams = new MediaMotiveTagBuildersParams
            {
                TileId = adSetting.TileId,
                AdSize = adSetting.AdSize,
                Make = displayAdsQuery.Make
            };

            var tags = _tagBuilders
                .Where(builder => builder.IsApplicable(mediaMotiveTagBuildersParams))
                .SelectMany(x => x.Build(mediaMotiveTagBuildersParams))
                .Where(x => !x.Name.IsNullOrEmpty())
                .Select(x => $"{x.Name}={string.Join(",", x.Values.NullSafe().Select(v => v.NullSafe()))}").Distinct().ToList();

            var urlargs = string.Join("/", tags);

            var dimensions = adSetting.AdSize.Dimensions().First();

            return new MediaMotiveAdViewModel()
            {
                TileId = adSetting.TileId.ToString(),
                Description = adSetting.Description.ToString(),
                Height = dimensions.Height,
                Width = dimensions.Width,
                DataKruxRequired = adSetting.DataKruxRequired,
                ScriptUrl = $"//mm.carsales.com.au/carsales/jserver/{urlargs}",
                NoScriptUrl = $"//mm.carsales.com.au/carsales/adclick/{urlargs}",
                NoScriptImageUrl = $"//mm.carsales.com.au/carsales/iserver/{urlargs}",
                DisplayAdsSource = DisplayAdsSource.MediaMotive
            };
        }

        private IDisplayAdsModel GetGoogleAdsModel(DisplayAdsQuery displayAdsQuery)
        {
            if (!DisplayAdsSettings.GoogleAdTypes.TryGetValue(displayAdsQuery.AdType, out var adSetting))
            {
                return null;
            }

            return new GoogleAdsViewModel()
            {
                Description = displayAdsQuery.AdType.ToString(),
                Dimensions = JsonConvert.SerializeObject(adSetting.AdSize.Dimensions()),
                AdNetworkCode = _tenantProvider.Current().GoogleAdsNetworkCode,
                AdUnitId = adSetting.UnitId,
                AdSlotId = adSetting.SlotId,
                DisplayAdsSource = DisplayAdsSource.GoogleAds
            };
        }
    }
}