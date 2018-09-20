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
                    DisplayAdSize = DisplayAdSizes.Leaderboard,
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
                    DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle,
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
                    DisplayAdSize = DisplayAdSizes.Block550X309,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes =
                        new List<string> {ArticleType.Sponsored.ToString(), ArticleType.Video.ToString(), ArticleType.Carpool.ToString()}
                }
            },
            {
                DisplayAdPlacements.Tracking,
                new MediaMotiveAdSetting()
                {
                    TileId = 20,
                    Description = DisplayAdPlacements.Tracking,
                    DisplayAdSize = DisplayAdSizes.Hidden,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>()
                }
            },
            {
                DisplayAdPlacements.Carousel,
                new MediaMotiveAdSetting()
                {
                    TileId = 3,
                    Description = DisplayAdPlacements.Carousel,
                    DisplayAdSize = DisplayAdSizes.MediumRectangle,
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
                    DisplayAdSize = DisplayAdSizes.Block1200X100,
                    DataKruxRequired = false,
                    NotSupportedArticleTypes = new List<string>()
                }
            }
        };
    }
}