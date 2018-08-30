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
                // This will be replaced by data requested from API //////////////////////
                if (!_typeToGoogleAdUnitId.TryGetValue(query.AdType, out string adUnitId))
                {
                    return null;
                }

                if (!_typeToGoogleAdSlotId.TryGetValue(query.AdType, out string adSlotId))
                {
                    return null;
                }
                
                if (!_typeToGoogleAdDimensions.TryGetValue(query.AdType, out string AdDimensions))
                {
                    return null;
                }
                //////////////////////////////////////////////////////////////////////////

                return new GoogleAdsViewModel()
                {
                    AdNetworkId = "5276053",
                    AdUnitId = adUnitId,
                    AdSlotId = adSlotId,
                    AdDimensions = AdDimensions,
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
                "div-gpt-ad-1468849624568-8"
            },
            {
                DisplayAdsTypes.Banner,
                "div-gpt-ad-1468849624568-5" // Use leaderboard size for now
            },
            {
                DisplayAdsTypes.LeaderBoard,
                "div-gpt-ad-1468849624568-5"
            },
            {
                DisplayAdsTypes.Mrec,
                "div-gpt-ad-1468849624568-2"
            }
        };

        private readonly Dictionary<DisplayAdsTypes, string> _typeToGoogleAdUnitId = new Dictionary<DisplayAdsTypes, string>()
        {
            {
                DisplayAdsTypes.Aside,
                "SA_Results_300x250_300x600_R4"
            },
            {
                DisplayAdsTypes.Banner,
                "SA_Homepage_728x90_M3_Top" // Use leaderboard unit for now
            },
            {
                DisplayAdsTypes.LeaderBoard,
                "SA_Homepage_728x90_M3_Top"
            },
            {
                DisplayAdsTypes.Mrec,
                "SA_Homepage_300x250_M4"
            }
        };

        private readonly Dictionary<DisplayAdsTypes, string> _typeToGoogleAdDimensions = new Dictionary<DisplayAdsTypes, string>()
        {
            {
                DisplayAdsTypes.Aside,
                ""
            },
            {
                DisplayAdsTypes.Banner,
                ""
            },
            {
                DisplayAdsTypes.LeaderBoard,
                ""
            },
            {
                DisplayAdsTypes.Mrec,
                ""
            }
        };
    }
}