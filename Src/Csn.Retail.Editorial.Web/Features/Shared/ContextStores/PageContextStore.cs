using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.WebMetrics.Core.Model;
using Ingress.Web.Common.Abstracts;

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
    }

    public class LandingPageContext : IPageContext
    {
        public PageContextTypes PageContextType =>  PageContextTypes.Listing;
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

    public interface IPageContextStore
    {
        IPageContext Get();
        void Set(IPageContext pageContext);
        bool Exists();
    }

    [AutoBind]
    public class PageContextStore : IPageContextStore
    {
        private readonly IContextStore _contextStore;

        public PageContextStore(IContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        public IPageContext Get()
        {
            var result = _contextStore.Get(ContextStoreKeys.CurrentPageContext.ToString());

            return result as IPageContext;
        }

        public void Set(IPageContext search)
        {
            _contextStore.Set(ContextStoreKeys.CurrentPageContext.ToString(), search);
        }

        public bool Exists()
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentPageContext.ToString());
        }
    }
}