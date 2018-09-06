using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public static class DisplayAdsSettings
    {
        public static Dictionary<DisplayAdsTypes, MediaMotiveAdSetting> MediaMotiveAdTypes = new Dictionary<DisplayAdsTypes, MediaMotiveAdSetting>
        {
            {
                DisplayAdsTypes.Leaderboard,
                new MediaMotiveAdSetting()
                {
                    TileId = 1,
                    Description = DisplayAdsTypes.Leaderboard,
                    AdSize = AdSize.Leaderboard,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdsTypes.Aside,
                new MediaMotiveAdSetting()
                {
                    TileId = 3,
                    Description = DisplayAdsTypes.Aside,
                    AdSize = AdSize.MediumOrLargeRectangle,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdsTypes.TEADS,
                new MediaMotiveAdSetting()
                {
                    TileId = 7,
                    Description = DisplayAdsTypes.TEADS,
                    AdSize = AdSize.Block550X309,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes =
                        new List<string> {ArticleType.Sponsored.ToString(), ArticleType.Video.ToString()}
                }
            },
            {
                DisplayAdsTypes.Tracking,
                new MediaMotiveAdSetting()
                {
                    TileId = 20,
                    Description = DisplayAdsTypes.Tracking,
                    AdSize = AdSize.Hidden,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string> {ArticleType.Sponsored.ToString()}
                }
            },
            {
                DisplayAdsTypes.Mrec,
                new MediaMotiveAdSetting()
                {
                    TileId = 3,
                    Description = DisplayAdsTypes.Mrec,
                    AdSize = AdSize.MediumRectangle,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdsTypes.Banner,
                new MediaMotiveAdSetting()
                {
                    TileId = 1,
                    Description = DisplayAdsTypes.Banner,
                    AdSize = AdSize.Block1200X100,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>()
                }
            }
        };


        public static Dictionary<DisplayAdsTypes, GoogleAdsSetting> GoogleAdTypes = new Dictionary<DisplayAdsTypes, GoogleAdsSetting>
        {
            {
                DisplayAdsTypes.Aside,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-8",
                    UnitId = "SA_Results_300x250_300x600_R4",
                    AdSize = AdSize.MediumOrLargeRectangle
                }
            },
            {
                DisplayAdsTypes.Banner,
                new GoogleAdsSetting
                {
                    // TODO:  Use leaderboard slot and unit for now
                    SlotId = "div-gpt-ad-1468849624568-5",
                    UnitId = "SA_Homepage_728x90_M3_Top",
                    AdSize = AdSize.Block1200X100
                }
            },
            {
                DisplayAdsTypes.Leaderboard,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-5",
                    UnitId = "SA_Homepage_728x90_M3_Top",
                    AdSize = AdSize.Leaderboard
                }
            },
            {
                DisplayAdsTypes.Mrec,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-2",
                    UnitId = "SA_Homepage_300x250_M4",
                    AdSize = AdSize.MediumRectangle
                }

            }
        };
    }
}