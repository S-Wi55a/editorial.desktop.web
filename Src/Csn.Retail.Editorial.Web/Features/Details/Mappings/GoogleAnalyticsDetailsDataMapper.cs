using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bolt.Common.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Ingress.Cache;
using Microsoft.Ajax.Utilities;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IGoogleAnalyticsDetailsDataMapper
    {
        GoogleAnalyticsDetailsData Map(ArticleDetailsDto article);
    }

    [AutoBind]
    public class GoogleAnalyticsDetailsDataMapper : IGoogleAnalyticsDetailsDataMapper
    {
        private readonly string _cacheKey = "editorial:desk:{0}:app:{1}";
        private readonly TimeSpan _localCacheDuration = new TimeSpan(0, 2, 0);
        private readonly TimeSpan _distributedCacheDuration = new TimeSpan(0, 15, 0);

        private readonly ICacheStore _cacheStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ITrackingApiProxy _trackingApiProxy;
        private readonly IUserContext _userContext;

        public GoogleAnalyticsDetailsDataMapper(ICacheStore cacheStore, ITrackingApiProxy trackingApiProxy,
            ITenantProvider<TenantInfo> tenantProvider, IUserContext userContext)
        {
            _cacheStore = cacheStore;
            _tenantProvider = tenantProvider;
            _trackingApiProxy = trackingApiProxy;
            _userContext = userContext;
        }


        public GoogleAnalyticsDetailsData Map(ArticleDetailsDto article)
        {
            var trackingResponse = new GoogleAnalyticsDetailsData
            {
                MemberTrackingId = _userContext.CurrentUserId,
                NetworkId = article.NetworkId,
                Make = article.Items.Any() ? string.Join(",", article.Items.Select(i => i.Make ?? string.Empty).Distinct()) : null,
                Model = article.Items.Any() ? string.Join(";", article.Items.Select(i => i.Model ?? string.Empty).Distinct()) : null,
                PublishDate = article.PublishDateTime,
                ContentGroup1 = "News and Reviews",
                ContentGroup2 = "details",
                ScriptBlock = GetCacheGaTracking()
            };

            return trackingResponse;
        }


        private string GetCacheGaTracking()
        {
            var cacheKey = _cacheKey.FormatWith(_tenantProvider.Current().Name, _tenantProvider.Current().GoogleAnalyticsApp);

            return _cacheStore.GetOrFetch(cacheKey, new CacheExpiredIn(_localCacheDuration, _distributedCacheDuration), GetGaTracking);
        }


        private string GetGaTracking()
        {
            var trackingResult = _trackingApiProxy.GetTracking(new TrackingApiInput
            {
                ApplicationName = _tenantProvider.Current().GoogleAnalyticsApp
            });

            if (trackingResult.TrackingScript.IsNullOrWhiteSpace())
                return null;

            return trackingResult.TrackingScript;
        }

    }
}