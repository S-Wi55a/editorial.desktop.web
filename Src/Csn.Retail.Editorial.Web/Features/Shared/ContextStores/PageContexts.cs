using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Listings.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.ContextStores
{
    public enum PageContextTypes
    {
        Listing,
        Landing,
        Details
    }

    public interface IPageContext
    {
        PageContextTypes PageContextType { get; }
    }

    public class DetailsPageContext : IPageContext
    {
        public PageContextTypes PageContextType =>  PageContextTypes.Details;
        public List<EditorialItem> Items { get; set; }
        public List<string> Lifestyles { get; set; }
        public List<string> Categories { get; set; }
        public string ArticleType { get; set; }
        public List<string> ArticleTypes { get; set; }
        public string Keywords { get; set; }
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