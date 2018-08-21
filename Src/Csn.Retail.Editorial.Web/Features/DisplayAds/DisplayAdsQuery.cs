using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public enum DisplayAdsTypes
    {
        Aside,
        Banner,
        LeaderBoard,
        Mrec
    }

    public class AdTypesDict
    {

    }

    public class DisplayAdsQuery : IQuery
    {
        public DisplayAdsTypes AdType { get; set; }
        /// <summary>
        /// Needed on Brand Page e.g. for Carouse & Hero sections on Landing page
        /// </summary>
        public string Make { get; set; }
    }
}