using System.Collections.Generic;

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
                                SlotId = "div-gpt-ad-1557777826935-0",
                                UnitId = "SA_editorial_300x250_300x600_E2",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.DetailsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1557777826935-0",
                                UnitId = "SA_editorial_300x250_300x600_E2",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.Banner,
                            new GoogleAdSetting
                            {
                                // TODO:  Use leaderboard slot ad unit for now
                                SlotId = "div-gpt-ad-1557777319543-0",
                                UnitId = "SA_editorial_728x90_E1",
                                DisplayAdSize = DisplayAdSizes.Block1200X100
                            }
                        },
                        {
                            DisplayAdPlacements.Leaderboard,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1557777319543-0",
                                UnitId = "SA_editorial_728x90_E1",
                                DisplayAdSize = DisplayAdSizes.Leaderboard
                            }
                        },
                        {
                            DisplayAdPlacements.Carousel,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1557777319543-0",
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
                                SlotId = "div-gpt-ad-1553688924791-0",
                                UnitId = "Editorial_Listings_Aside(300x250_300x600)",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.DetailsAside,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1553689007338-0",
                                UnitId = "Editorial_Details_Aside(300x250_300x600)",
                                DisplayAdSize = DisplayAdSizes.MediumOrLargeRectangle
                            }
                        },
                        {
                            DisplayAdPlacements.Banner,
                            new GoogleAdSetting
                            {
                                // TODO:  Use leaderboard slot ad unit for now
                                SlotId = "div-gpt-ad-1553524316965-0",
                                UnitId = "Editorial_HomePage_728x90",
                                DisplayAdSize = DisplayAdSizes.Block1200X100
                            }
                        },
                        {
                            DisplayAdPlacements.Leaderboard,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1553524424201-0",
                                UnitId = "DetailsPage_728x90",
                                DisplayAdSize = DisplayAdSizes.Leaderboard
                            }
                        },
                        {
                            DisplayAdPlacements.Carousel,
                            new GoogleAdSetting
                            {
                                SlotId = "div-gpt-ad-1553524510647-0",
                                UnitId = "Editorial_HomePage_300x250",
                                DisplayAdSize = DisplayAdSizes.MediumRectangle
                            }

                        }
                    }
                }
            };
    }
}