using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Ingress.Web.Common.Abstracts;

namespace Csn.Retail.Editorial.Web.Features.Shared.ContextStores
{
    public class RyvussSearch
    {
        public RyvussNavResultDto RyvussNavResult { get; set; }
        public SearchContext SearchContext { get; set; }
    }

    public class SearchContext
    {
        public string Query { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string SeoFragment { get; set; }
    }

    public interface ISearchResultContextStore
    {
        RyvussSearch Get();
        void Set(RyvussSearch searchResult);
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

        public RyvussSearch Get()
        {
            var result = _contextStore.Get(ContextStoreKeys.CurrentSearchResult.ToString());

            return result as RyvussSearch;
        }

        public void Set(RyvussSearch search)
        {
            _contextStore.Set(ContextStoreKeys.CurrentSearchResult.ToString(), search);
        }

        public bool Exists()
        {
            return _contextStore.Exists(ContextStoreKeys.CurrentSearchResult.ToString());
        }
    }
}