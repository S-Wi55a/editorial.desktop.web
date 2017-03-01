using System;
using System.Linq;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.TrackingApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;
using Ingress.Cache;
using IContextStore = Csn.Retail.Editorial.Web.Infrastructure.ContextStores.IContextStore;

namespace Csn.Retail.Editorial.Web.Features.Tracking.GoogleAnalytics
{
    [AutoBind]
    public class GoogleAnalyticsDetailsQueryHandler : IQueryHandler<GoogleAnalyticsDetailsQuery, GoogleAnalyticsDetailsModel>
    {
        private const string Key = "GA.Tracking.Editorial";

        private readonly IContextStore _contextStore;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ITrackingApiProxy _trackingApiProxy;
        private readonly IUserContext _userContext;

        public GoogleAnalyticsDetailsQueryHandler(IContextStore contextStore, ITrackingApiProxy trackingApiProxy,
            ITenantProvider<TenantInfo> tenantProvider, IUserContext userContext)
        {
            _contextStore = contextStore;
            _tenantProvider = tenantProvider;
            _trackingApiProxy = trackingApiProxy;
            _userContext = userContext;
        }


        public GoogleAnalyticsDetailsModel Handle(GoogleAnalyticsDetailsQuery query)
        {
            var article = query.Article;

            var trackingResponse = new GoogleAnalyticsDetailsModel
            {
                NetworkId = article.NetworkId,
                Make = article.Items.Any() ? string.Join(",", article.Items.Select(i => i.Make ?? string.Empty).Distinct()) : null,
                Model = article.Items.Any() ? string.Join(";", article.Items.Select(i => i.Model ?? string.Empty).Distinct()) : null,
                PublishDate = Convert.ToDateTime(article.DateAvailable).ToString("s"),
                ContentGroup1 = "News and Reviews",
                ContentGroup2 = query.PageType
            };

            var scriptBlock = GetGaTracking(query);

            return trackingResponse;
        }


        private string GetGaTracking(GoogleAnalyticsDetailsQuery query)
        {
            var trackingResult = _trackingApiProxy.GetTracking(new TrackingApiInput
            {
                ApplicationName = _tenantProvider.Current().GoogleAnalyticsApp
            });

            if (trackingResult == null)
                return null;

            return _contextStore.GetOrFetch(Key, () => GetGaTracking(query));
        }
    }
}