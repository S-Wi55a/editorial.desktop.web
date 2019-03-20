using System;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Ingress.Cache;

namespace Csn.Retail.Editorial.Web.Features.Details.CacheStores
{
    public interface IArticleDetailsCacheStore
    {
        Task<MayBe<GetArticleResponse>> GetAsync(string networkId, CachePageType cachePageType);
        Task StoreAsync(GetArticleResponse article, CachePageType cachePageType);
    }

    [AutoBind]
    public class ArticleDetailsCacheStore : IArticleDetailsCacheStore
    {
        private readonly string _cacheKey = "editorial:desk:{0}:{1}:{2}:{3}";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 5, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 30, 0);
        private readonly string _buildVersion = System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];

        private readonly ICacheStore _cacheStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public ArticleDetailsCacheStore(ICacheStore cacheStore, ITenantProvider<TenantInfo> tenantProvider)
        {
            _cacheStore = cacheStore;
            _tenantProvider = tenantProvider;
        }

        public Task<MayBe<GetArticleResponse>> GetAsync(string networkId, CachePageType cachePageType)
        {
            return _cacheStore.GetAsync<GetArticleResponse>(GetCacheKey(networkId, cachePageType));
        }

        public Task StoreAsync(GetArticleResponse article, CachePageType cachePageType)
        {
            if (article != null && !string.IsNullOrEmpty(article.ArticleViewModel?.NetworkId))
            {
                return _cacheStore.SetAsync(GetCacheKey(article.ArticleViewModel.NetworkId, cachePageType), article, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration));
            }

            return Task.CompletedTask;
        }

        private string GetCacheKey(string networkId, CachePageType cachePageType)
        {
            return _cacheKey.FormatWith(_buildVersion, _tenantProvider.Current().Name, cachePageType.ToString(), networkId);
        }
    }

    public enum CachePageType
    {
        Details,
        Modal
    }
}