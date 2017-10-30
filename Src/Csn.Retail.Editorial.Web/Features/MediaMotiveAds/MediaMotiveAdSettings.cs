using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public static class MediaMotiveAdSettings
    {
        public static Dictionary<string, MediaMotiveAdSetting> AdTypes = new Dictionary<string, MediaMotiveAdSetting>
        {
            {
                "Tile1",
                new MediaMotiveAdSetting() {
                    Id = "1", Description = MediaMotiveAdType.Leaderboard, AdSize = AdSize.Leaderboard, DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>{}
                }
            },
            {
                "Tile3",
                new MediaMotiveAdSetting() {
                    Id = "3", Description = MediaMotiveAdType.MREC, AdSize = AdSize.MediumOrLargeRectangle, DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>{}
                }
            },
            {
                "Tile7",
                new MediaMotiveAdSetting() {
                    Id = "7", Description = MediaMotiveAdType.TEADS, AdSize = AdSize.Block550X309, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString(), ArticleType.Video.ToString()}
                }
            },
            {
                "Tile9",
                new MediaMotiveAdSetting() {
                    Id = "9", Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                "Tile10",
                new MediaMotiveAdSetting() {
                    Id = "10", Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                "Tile11",
                new MediaMotiveAdSetting() {
                    Id = "11", Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                "Tile12",
                new MediaMotiveAdSetting() {
                    Id = "12", Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                "Tile13",
                new MediaMotiveAdSetting() {
                    Id = "13", Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                "Tile20",
                new MediaMotiveAdSetting() {
                    Id = "20", Description = MediaMotiveAdType.Tracking, AdSize = AdSize.Hidden, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
        };
    }
}