using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.SimpleCqrs;
using Ingress.Cache;
using Microsoft.Ajax.Utilities;

namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public interface IRequiredGoogleAnalyticsTrackingScript { }

    public class LoadGoogleAnalyticsTrackingScriptOnPageLoad<T> : IAsyncEventHandler<T> where T : IEvent
    {
        private readonly string _cacheKey = "editorial:desktop:ga:{0}";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 30, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(24, 0, 0);

        private readonly ICacheStore _cacheStore;
        private readonly IContextStore<TrackingApiDto> _contextStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ITrackingApiProxy _trackingApiProxy;

        public LoadGoogleAnalyticsTrackingScriptOnPageLoad(ICacheStore cacheStore,
            IContextStore<TrackingApiDto> contextStore, ITenantProvider<TenantInfo> tenantProvider,
            ITrackingApiProxy trackingApiProxy)
        {
            _cacheStore = cacheStore;
            _contextStore = contextStore;
            _tenantProvider = tenantProvider;
            _trackingApiProxy = trackingApiProxy;
        }

        public async Task HandleAsync(T eEvent)
        {
            if (!(eEvent is IRequiredGoogleAnalyticsTrackingScript)) return;

            var cacheKey = _cacheKey.FormatWith(_tenantProvider.Current().GoogleAnalyticsApp);

            var response = await _cacheStore.OnCacheMiss(GetGaTracking)
                        .CacheIf(x => !string.IsNullOrWhiteSpace(x?.TrackingScript))
                        .Duration(_localCacheDuration, _distributedCacheDuration)
                        .GetAsync(cacheKey);

            _contextStore.Set(response);
        }

        private async Task<TrackingApiDto> GetGaTracking()
        {
            var trackingResult = await _trackingApiProxy.GetTracking(new TrackingApiInput
            {
                ApplicationName = _tenantProvider.Current().GoogleAnalyticsApp
            });

            return trackingResult.IsSucceed && trackingResult.Data.TrackingScript.IsNullOrWhiteSpace() ? null : trackingResult.Data;
        }
    }
}