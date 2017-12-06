using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.WebMetrics.Core.Model;
using Ingress.Web.Common.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.ContextStores
{
    public class SearchContext
    {
        public RyvussNavResultDto RyvussNavResult { get; set; }
        public string Query { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string SeoFragment { get; set; }
        public SearchEventType SearchEventType { get; set; }
        public EditorialPageTypes EditorialPageType { get; set; }
    }

    public interface ISearchResultContextStore
    {
        SearchContext Get();
        void Set(SearchContext searchResult);
        bool Exists();
    }

    [AutoBind]
    public class SearchResultContextStore : ISearchResultContextStore
    {
        private readonly IContextStore _contextStore;

        public SearchResultContextStore(IContextStore contextStore)
        {
            _contextStore = contextStore;
        }

        public SearchContext Get()
        {
            var result = _contextStore.Get(ContextStoreKeys.CurrentSearchResult.ToString());

            return result as SearchContext;
        }

        public void Set(SearchContext search)
        {
            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), search);
        }

        public bool Exists()
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentSearchResult.ToString());
        }
    }
}