using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.GoogleAd
{
    public class GoogleAdSettings
    {
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
                    // TODO:  Use leaderboard slot ad unit for now
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