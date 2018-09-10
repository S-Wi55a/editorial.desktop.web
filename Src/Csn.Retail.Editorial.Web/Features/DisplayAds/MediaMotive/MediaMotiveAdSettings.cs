﻿using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive
{
    public class MediaMotiveAdSettings
    {
        public static Dictionary<DisplayAdPlacements, MediaMotiveAdSetting> MediaMotiveAdTypes = new Dictionary<DisplayAdPlacements, MediaMotiveAdSetting>
        {
            {
                DisplayAdPlacements.Leaderboard,
                new MediaMotiveAdSetting()
                {
                    TileId = 1,
                    Description = DisplayAdPlacements.Leaderboard,
                    DisplayAdsAdSize = DisplayAdsAdSize.Leaderboard,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdPlacements.Aside,
                new MediaMotiveAdSetting()
                {
                    TileId = 3,
                    Description = DisplayAdPlacements.Aside,
                    DisplayAdsAdSize = DisplayAdsAdSize.MediumOrLargeRectangle,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdPlacements.TEADS,
                new MediaMotiveAdSetting()
                {
                    TileId = 7,
                    Description = DisplayAdPlacements.TEADS,
                    DisplayAdsAdSize = DisplayAdsAdSize.Block550X309,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes =
                        new List<string> {ArticleType.Sponsored.ToString(), ArticleType.Video.ToString()}
                }
            },
            {
                DisplayAdPlacements.Tracking,
                new MediaMotiveAdSetting()
                {
                    TileId = 20,
                    Description = DisplayAdPlacements.Tracking,
                    DisplayAdsAdSize = DisplayAdsAdSize.Hidden,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string> {ArticleType.Sponsored.ToString()}
                }
            },
            {
                DisplayAdPlacements.Carousel,
                new MediaMotiveAdSetting()
                {
                    TileId = 3,
                    Description = DisplayAdPlacements.Carousel,
                    DisplayAdsAdSize = DisplayAdsAdSize.MediumRectangle,
                    DataKruxRequired = true,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdPlacements.Banner,
                new MediaMotiveAdSetting()
                {
                    TileId = 1,
                    Description = DisplayAdPlacements.Banner,
                    DisplayAdsAdSize = DisplayAdsAdSize.Block1200X100,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>()
                }
            }
        };
    }
}