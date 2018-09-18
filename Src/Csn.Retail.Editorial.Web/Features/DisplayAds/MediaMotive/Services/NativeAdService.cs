using System;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.Models;
using Ingress.Cache;
using Ingress.ServiceClient.Abstracts;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.Services
{
    public interface INativeAdService
    {
        string GetNativeAdvert();
    }

    [AutoBind]
    public class NativeAdService : INativeAdService
    {
        private readonly ISmartServiceClient _client;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ICacheStore _cacheStore;
        private readonly string _cacheKey = "editorial:desk:{0}:nativeAd";
        private const string ServiceName = "api-native-adserver-aws";

        public NativeAdService(ICacheStore cacheStore, ISmartServiceClient client, ITenantProvider<TenantInfo> tenantProvider)
        {
            _cacheStore = cacheStore;
            _client = client;
            _tenantProvider = tenantProvider;
        }

        public string GetNativeAdvert()
        {
            if (string.IsNullOrEmpty(_tenantProvider.Current().NativeAdPlacement)) return null;

            var cacheKey = _cacheKey.FormatWith(_tenantProvider.Current().Name);

            var addResults = _cacheStore.OnCacheMiss(GetAdvertContentAsync)
                .CacheIf(x => !string.IsNullOrWhiteSpace(x))
                .LocalCacheDuration(TimeSpan.FromSeconds(200))
                .Get(cacheKey);

            return addResults;

        }

        private string GetAdvertContentAsync()
        {
            var response = _client.Service(ServiceName)
                .Path($"/v1/adverts?placement={_tenantProvider.Current().NativeAdPlacement}")
                .Get<NativeAdResponse>();

            return (response.IsSucceed && response.Data != null) ? response.Data.Content : null;
        }
    }
}