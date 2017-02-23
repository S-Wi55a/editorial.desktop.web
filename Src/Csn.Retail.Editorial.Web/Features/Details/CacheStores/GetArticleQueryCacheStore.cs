﻿using System;
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
    public class GetArticleQueryCacheStore : IAsyncCacheStore<GetArticleQuery, ArticleViewModel>
    {
        private readonly ICacheStore _cacheStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly string _cacheKey = "editorial:desk:{0}:details:{1}";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 2, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 15, 0);

        public GetArticleQueryCacheStore(ICacheStore cacheStore, ITenantProvider<TenantInfo> tenantProvider)
        {
            _cacheStore = cacheStore;
            _tenantProvider = tenantProvider;
        }

        public async Task<ArticleViewModel> GetAsync(GetArticleQuery query, Func<GetArticleQuery, Task<ArticleViewModel>> fetchAsync)
        {
            var cacheKey = _cacheKey.FormatWith(_tenantProvider.Current().Name, query.Id);

            // check the cache
            return await _cacheStore.GetOrFetchAsync(cacheKey, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration), () => fetchAsync(query));
        }
    }
}