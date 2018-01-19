using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public static class MediaMotiveAdSettings
    {
        public static Dictionary<AdSize, MediaMotiveAdSetting> AdTypes = new Dictionary<AdSize, MediaMotiveAdSetting>
        {
            {
                AdSize.Leaderboard,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.Leaderboard, AdSize = AdSize.Leaderboard, DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>{}
                }
            },
            {
                AdSize.MediumOrLargeRectangle,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.MREC, AdSize = AdSize.MediumOrLargeRectangle, DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>{}
                }
            },
            {
                AdSize.Block550X309,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.TEADS, AdSize = AdSize.Block550X309, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString(), ArticleType.Video.ToString()}
                }
            },
            {
                AdSize.SponsoredLink,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.SponsoredLink, AdSize = AdSize.SponsoredLink, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                AdSize.Hidden,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.Tracking, AdSize = AdSize.Hidden, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{ArticleType.Sponsored.ToString()}
                }
            },
            {
                AdSize.MediumRectangle,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.MREC, AdSize = AdSize.MediumRectangle, DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>{}
                }
            },
            {
                AdSize.Block1200X100,
                new MediaMotiveAdSetting() {
                    Description = MediaMotiveAdType.Banner, AdSize = AdSize.Block1200X100, DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>{}
                }
            }
        };
    }
}