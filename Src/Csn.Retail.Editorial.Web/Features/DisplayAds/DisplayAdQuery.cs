using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public enum DisplayAdPlacements
    {
        Aside,
        Banner,
        Leaderboard,
        Carousel,
        TEADS,
        Tracking
    }

    public class DisplayAdQuery : IQuery
    {
        public DisplayAdPlacements AdPlacement { get; set; }
        public string Make { get; set; }
    }
}