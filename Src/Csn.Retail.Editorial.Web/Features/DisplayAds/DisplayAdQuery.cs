using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public enum DisplayAdPlacements
    {
        ListingsAside,
        Banner,
        Leaderboard,
        Carousel,
        TEADS,
        Tracking,
        InlineBanner
    }

    public class DisplayAdQuery : IQuery
    {
        public DisplayAdPlacements AdPlacement { get; set; }
        public string Make { get; set; }
    }
}