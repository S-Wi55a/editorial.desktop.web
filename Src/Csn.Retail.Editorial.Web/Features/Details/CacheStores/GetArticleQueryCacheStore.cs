using System;
using System.Net;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended;
using Ingress.Cache;

namespace Csn.Retail.Editorial.Web.Features.Details.CacheStores
{
    [AutoBind]
    public class GetArticleQueryCacheStore : IAsyncCacheStore<GetArticleQuery, GetArticleResponse>
    {
        private readonly ICacheStore _cacheStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly string _cacheKey = "editorial:desk:{0}:{1}:details:{2}";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 5, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 30, 0);
        private readonly string _buildVersion = System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];

        public GetArticleQueryCacheStore(ICacheStore cacheStore, ITenantProvider<TenantInfo> tenantProvider)
        {
            _cacheStore = cacheStore;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetArticleResponse> GetAsync(GetArticleQuery query, Func<GetArticleQuery, Task<GetArticleResponse>> fetchAsync)
        {
            if (query.IsPreview)
            {
                // bypass cache for articles which are being previewed
                return await fetchAsync.Invoke(query);
            }

            var cacheKey = _cacheKey.FormatWith(_buildVersion, _tenantProvider.Current().Name, query.Id);

            // check the cache
            var cachedArticle = await _cacheStore.GetAsync<ArticleViewModel>(cacheKey);

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
                await _cacheStore.SetAsync(cacheKey, result.ArticleViewModel, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration));
            }

            return result;
        }
    }
}