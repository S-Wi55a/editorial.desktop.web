using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.Models;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds;
using Csn.Retail.Editorial.Web.Features.MediaMotiveAds.TagBuilders;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    [AutoBind]
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
                if (!_typeToMediaMotiveAdQuery.TryGetValue(query.AdType, out MediaMotiveAdQuery mediaMotiveAdQuery))
                {
                    return null;
                }

                if (!query.Make.IsNullOrEmpty())
                {
                    mediaMotiveAdQuery.Make = query.Make;
                }

                var tags = _tagBuilders
                    .Where(builder => builder.IsApplicable(mediaMotiveAdQuery))
                    .SelectMany(x => x.Build(mediaMotiveAdQuery))
                    .Where(x => !x.Name.IsNullOrEmpty())
                    .Select(x => $"{x.Name}={string.Join(",", x.Values.NullSafe().Select(v => v.NullSafe()))}").Distinct().ToList();

                var urlargs = string.Join("/", tags);

                // lookup the ad settings for this tile
                if (!MediaMotiveAdSettings.AdTypes.TryGetValue(mediaMotiveAdQuery.AdSize, out MediaMotiveAdSetting adSetting))
                {
                    return null;
                }

                var dimensions = adSetting.AdSize.Dimensions().First();

                return new MediaMotiveAdViewModel()
                {
                    TileId = mediaMotiveAdQuery.TileId.ToString(),
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
            if (_tenantProvider.Current().AdSource == "GoogleAds")
            {
                if (!_typeToGoogleAdSlotId.TryGetValue(query.AdType, out string _slotId))
                {
                    return null;
                }
                return new GoogleAdsViewModel()
                {
                    SlotId = _slotId,
                    Description = query.AdType.ToString(),
                    DisplayAdsSource = DisplayAdsSource.GoogleAds
                };
            }

            return null;
        }
        
        private readonly Dictionary<DisplayAdsTypes, MediaMotiveAdQuery> _typeToMediaMotiveAdQuery = new Dictionary<DisplayAdsTypes, MediaMotiveAdQuery>()
        {
            {
                DisplayAdsTypes.Aside,
                new MediaMotiveAdQuery() { AdSize = AdSize.MediumOrLargeRectangle, TileId = 3 }
            },
            {
                DisplayAdsTypes.Banner,
                new MediaMotiveAdQuery() { AdSize = AdSize.Block1200X100, TileId = 1 }
            },
            {
                DisplayAdsTypes.LeaderBoard,
                new MediaMotiveAdQuery() { AdSize = AdSize.Leaderboard, TileId = 1 }
            },
            {
                DisplayAdsTypes.Mrec,
                new MediaMotiveAdQuery() { AdSize = AdSize.MediumRectangle, TileId = 3 }
            }
        };

        private readonly Dictionary<DisplayAdsTypes, string> _typeToGoogleAdSlotId = new Dictionary<DisplayAdsTypes, string>()
        {
            {
                DisplayAdsTypes.Aside,
                "1468849624568-8"
            },
            {
                DisplayAdsTypes.Banner,
                "1468849624568-5"
            },
            {
                DisplayAdsTypes.LeaderBoard,
                "1468849624568-5"
            },
            {
                DisplayAdsTypes.Mrec,
                "1468849624568-2"
            }
        };
    }
}