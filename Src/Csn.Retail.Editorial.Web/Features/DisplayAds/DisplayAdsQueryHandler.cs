using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public class DisplayAdsQueryHandler : IQueryHandler<DisplayAdsQuery, IDisplayAdsModel>
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IEnumerable<IMediaMotiveTagBuilder> _tagBuilders;

        public DisplayAdsQueryHandler(ITenantProvider<TenantInfo> tenantProvider, IEnumerable<IMediaMotiveTagBuilder> tagBuilders)
        {
            _tenantProvider = tenantProvider;
            _tagBuilders = tagBuilders;
        }
        public IDisplayAdsModel Handle(DisplayAdsQuery query)
        {
            if (_tenantProvider.Current().AdSource == "MediaMotive")
            {
                var mediaMotiveQuery = new MediaMotiveAdQuery()
                {
                    TileId = 3,
                    AdSize = AdSize.MediumRectangle,
                    Make = query.Make
                };
                var tags = _tagBuilders
                    .Where(builder => builder.IsApplicable(mediaMotiveQuery))
                    .SelectMany(x => x.Build(mediaMotiveQuery))
                    .Where(x => !x.Name.IsNullOrEmpty())
                    .Select(x => $"{x.Name}={string.Join(",", x.Values.NullSafe().Select(v => v.NullSafe()))}").Distinct().ToList();

                var urlargs = string.Join("/", tags);

                // lookup the ad settings for this tile
                if (!MediaMotiveAdSettings.AdTypes.TryGetValue(query.AdSize, out MediaMotiveAdSetting adSetting))
                {
                    return null;
                }

                var dimensions = adSetting.AdSize.Dimensions().First();

                return new MediaMotiveAdViewModel()
                {
                    TileId = query.TileId.ToString(),
                    Description = adSetting.Description.ToString(),
                    Height = dimensions.Height,
                    Width = dimensions.Width,
                    DataKruxRequired = adSetting.DataKruxRequired,
                    ScriptUrl = $"//mm.carsales.com.au/carsales/jserver/{urlargs}",
                    NoScriptUrl = $"//mm.carsales.com.au/carsales/adclick/{urlargs}",
                    NoScriptImageUrl = $"//mm.carsales.com.au/carsales/iserver/{urlargs}"
                };


                new MediaMotiveAdViewModel()
                {

                }
            }
            if (_tenantProvider.Current().AdSource == "GoogleAds")
            {

            }
        }
    }
}