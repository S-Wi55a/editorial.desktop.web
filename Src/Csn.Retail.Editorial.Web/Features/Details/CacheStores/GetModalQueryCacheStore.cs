using System;
using System.Net;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended;

namespace Csn.Retail.Editorial.Web.Features.Details.CacheStores
{
    [AutoBind]
    public class GetModalQueryCacheStore : IAsyncCacheStore<GetModalQuery, GetArticleResponse>
    {
        private readonly IArticleDetailsCacheStore _articleDetailsCacheStore;

        public GetModalQueryCacheStore(IArticleDetailsCacheStore articleDetailsCacheStore)
        {
            _articleDetailsCacheStore = articleDetailsCacheStore;
        }

        public async Task<GetArticleResponse> GetAsync(GetModalQuery query, Func<GetModalQuery, Task<GetArticleResponse>> fetchAsync)
        {
            // check the cache
            var cachedArticle = await _articleDetailsCacheStore.GetAsync(query.Id, CachePageType.Modal);

            if (cachedArticle.HasValue)
            {
                return cachedArticle.Value;
            }

            // otherwise fetch the data
            var result = await fetchAsync.Invoke(query);

            // store the result in cache if required
            if (result.ArticleViewModel != null && result.HttpStatusCode == HttpStatusCode.OK)
            {
                await _articleDetailsCacheStore.StoreAsync(result, CachePageType.Modal);
            }

            return result;
        }
    }
}