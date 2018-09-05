using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public enum DisplayAdsTypes
    {
        Aside,
        Banner,
        Leaderboard,
        Mrec,
        TEADS,
        Tracking
    }

    public class DisplayAdsQuery : IQuery
    {
        public DisplayAdsTypes AdType { get; set; }
        public string Make { get; set; }
    }
}