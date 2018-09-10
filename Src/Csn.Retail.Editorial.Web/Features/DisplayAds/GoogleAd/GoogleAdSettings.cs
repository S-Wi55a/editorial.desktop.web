using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd
{
    public class GoogleAdSettings
    {
        public static Dictionary<DisplayAdPlacements, GoogleAdsSetting> GoogleAdTypes = new Dictionary<DisplayAdPlacements, GoogleAdsSetting>
        {
            {
                DisplayAdPlacements.Aside,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-8",
                    UnitId = "SA_Results_300x250_300x600_R4",
                    DisplayAdsAdSize = DisplayAdsAdSize.MediumOrLargeRectangle
                }
            },
            {
                DisplayAdPlacements.Banner,
                new GoogleAdsSetting
                {
                    // TODO:  Use leaderboard slot ad unit for now
                    SlotId = "div-gpt-ad-1468849624568-5",
                    UnitId = "SA_Homepage_728x90_M3_Top",
                    DisplayAdsAdSize = DisplayAdsAdSize.Block1200X100
                }
            },
            {
                DisplayAdPlacements.Leaderboard,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-5",
                    UnitId = "SA_Homepage_728x90_M3_Top",
                    DisplayAdsAdSize = DisplayAdsAdSize.Leaderboard
                }
            },
            {
                DisplayAdPlacements.Carousel,
                new GoogleAdsSetting
                {
                    SlotId = "div-gpt-ad-1468849624568-2",
                    UnitId = "SA_Homepage_300x250_M4",
                    DisplayAdsAdSize = DisplayAdsAdSize.MediumRectangle
                }

            }
        };

    }
}