using System;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Landing.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs.Extended;
using Ingress.Cache;

namespace Csn.Retail.Editorial.Web.Features.Landing.CacheStores
{
    [AutoBind]
    public class GetLandingQueryCacheStore : IAsyncCacheStore<GetLandingQuery, GetLandingResponse>
    {
        private readonly ICacheStore _cacheStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly string _cacheKey = "editorial:desk:{0}:{1}:landingViewModel";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 5, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 10, 0);
        private readonly string _buildVersion = System.Configuration.ConfigurationManager.AppSettings["BuildVersion"];

        public GetLandingQueryCacheStore(ICacheStore cacheStore, ITenantProvider<TenantInfo> tenantProvider)
        {
            _cacheStore = cacheStore;
            _tenantProvider = tenantProvider;
        }
        
        public async Task<GetLandingResponse> GetAsync(GetLandingQuery query, Func<GetLandingQuery, Task<GetLandingResponse>> fetchAsync)
        {
            var cacheKey = _cacheKey.FormatWith(_buildVersion, _tenantProvider.Current().Name);

            // check the cache
            var cachedViewModel = await _cacheStore.GetAsync<LandingViewModel>(cacheKey);

            if (cachedViewModel.HasValue && !query.PromotionId.HasValue)
            {
                return new GetLandingResponse
                {
                    LandingViewModel = cachedViewModel.Value
                };
            }

            // otherwise fetch the data
            var result = await fetchAsync.Invoke(query);

            // store the result in cache if required
            if (result?.LandingViewModel != null && result.CacheViewModel)
            {
                await _cacheStore.SetAsync(cacheKey, result.LandingViewModel, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration));
            }

            return result;
        }
    }
}