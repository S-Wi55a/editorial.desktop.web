using System;
using System.Net;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended;

namespace Csn.Retail.Editorial.Web.Features.Details.CacheStores
{
    [AutoBind]
    public class GetArticleQueryCacheStore : IAsyncCacheStore<GetArticleQuery, GetArticleResponse>
    {
        private readonly IArticleDetailsCacheStore _articleDetailsCacheStore;

        public GetArticleQueryCacheStore(IArticleDetailsCacheStore articleDetailsCacheStore)
        {
            _articleDetailsCacheStore = articleDetailsCacheStore;
        }

        public async Task<GetArticleResponse> GetAsync(GetArticleQuery query, Func<GetArticleQuery, Task<GetArticleResponse>> fetchAsync)
        {
            if (query.IsPreview)
            {
                // bypass cache for articles which are being previewed
                return await fetchAsync.Invoke(query);
            }

            // check the cache
            var cachedArticle = await _articleDetailsCacheStore.GetAsync(query.Id);

            if (cachedArticle.HasValue)
            {
                return cachedArticle.Value;
            }

            // otherwise fetch the data
            var result = await fetchAsync.Invoke(query);

            // store the result in cache if required
            if (result.ArticleViewModel != null && result.HttpStatusCode == HttpStatusCode.OK)
            {
                await _articleDetailsCacheStore.StoreAsync(result);
            }

            return result;
        }
    }
}