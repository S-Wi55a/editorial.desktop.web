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
        private readonly IArticleViewModelCacheStore _articleViewModelCacheStore;

        public GetArticleQueryCacheStore(IArticleViewModelCacheStore articleViewModelCacheStore)
        {
            _articleViewModelCacheStore = articleViewModelCacheStore;
        }

        public async Task<GetArticleResponse> GetAsync(GetArticleQuery query, Func<GetArticleQuery, Task<GetArticleResponse>> fetchAsync)
        {
            if (query.IsPreview)
            {
                // bypass cache for articles which are being previewed
                return await fetchAsync.Invoke(query);
            }

            // check the cache
            var cachedArticle = await _articleViewModelCacheStore.GetAsync(query.Id);

            if (cachedArticle.HasValue)
            {
                return new GetArticleResponse()
                {
                    ArticleViewModel = cachedArticle.Value,
                    HttpStatusCode = HttpStatusCode.OK
                };
            }

            // otherwise fetch the data
            var result = await fetchAsync.Invoke(query);

            // store the result in cache if required
            if (result.ArticleViewModel != null)
            {
                await _articleViewModelCacheStore.StoreAsync(result.ArticleViewModel);
            }

            return result;
        }
    }
}