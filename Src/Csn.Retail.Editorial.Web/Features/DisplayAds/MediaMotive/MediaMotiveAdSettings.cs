using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    public class MediaMotiveAdSettings
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
    }
}