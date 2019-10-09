using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.ContextStores
{
    public enum PageContextTypes
    {
        Listing,
        Landing
    }

    public interface IPageContext
    {
        PageContextTypes PageContextType { get; }
    }

    public class LandingPageContext : IPageContext
    {
        public PageContextTypes PageContextType =>  PageContextTypes.Landing;
        public string Make { get; set; }
    }

    public class ListingPageContext : IPageContext
    {
        public PageContextTypes PageContextType =>  PageContextTypes.Listing;
        public RyvussNavResultDto RyvussNavResult { get; set; }
        public string Query { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string SeoFragment { get; set; }
        public SearchEventType SearchEventType { get; set; }
        public EditorialPageTypes EditorialPageType { get; set; }
    }
}