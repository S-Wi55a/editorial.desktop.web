﻿using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd
{
    public class GoogleAdSettings
    {
        public static Dictionary<string, Dictionary<DisplayAdPlacements, GoogleAdSetting>> GoogleAdTypes =
            new Dictionary<string, Dictionary<DisplayAdPlacements, GoogleAdSetting>>
            {
                {
                    "soloautos",
                    new Dictionary<DisplayAdPlacements, GoogleAdSetting>
                    {
                        {
                            DisplayAdPlacements.ListingsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1548199795543-0",
                                UnitId = "SA_editorial_300x250_300x600_E3",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.DetailsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1548093016580-0'",
                                UnitId = "SA_editorial_300x250_300x600_E2",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.Banner,
                            new GoogleAdSetting
                            {
                                // TODO:  Use leaderboard slot ad unit for now
                                SlotId = "div-gpt-ad-1468849624568-5",
                                UnitId = "SA_Homepage_728x90_M3_Top",
                                DisplayAdSize = DisplayAdSizes.Block1200X100
                            }
                        },
                        {
                            DisplayAdPlacements.Leaderboard,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1496703639902-0",
                                UnitId = "SA_editorial_728x90_E1",
                                DisplayAdSize = DisplayAdSizes.Leaderboard
                            }
                        },
                        {
                            DisplayAdPlacements.Carousel,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1496704783581-0",
                                UnitId = "SA_editorial_300x250_E2",
                                DisplayAdSize = DisplayAdSizes.MediumRectangle
                            }

                        }
                    }
                },
                {
                    "chileautos",
                    new Dictionary<DisplayAdPlacements, GoogleAdSetting>
                    {
                        {
                            DisplayAdPlacements.ListingsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1548199795543-0",
                                UnitId = "SA_editorial_300x250_300x600_E3",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.DetailsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1548093016580-0'",
                                UnitId = "SA_editorial_300x250_300x600_E2",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.Banner,
                            new GoogleAdSetting
                            {
                                // TODO:  Use leaderboard slot ad unit for now
                                SlotId = "div-gpt-ad-1468849624568-5",
                                UnitId = "SA_Homepage_728x90_M3_Top",
                                DisplayAdSize = DisplayAdSizes.Block1200X100
                            }
                        },
                        {
                            DisplayAdPlacements.Leaderboard,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1496703639902-0",
                                UnitId = "SA_editorial_728x90_E1",
                                DisplayAdSize = DisplayAdSizes.Leaderboard
                            }
                        },
                        {
                            DisplayAdPlacements.Carousel,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1496704783581-0",
                                UnitId = "SA_editorial_300x250_E2",
                                DisplayAdSize = DisplayAdSizes.MediumRectangle
                            }

                        }
                    }
                }
            };
    }
}