using System;
using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Cars.Cache;
using Csn.Cars.Cache.Extensions;
using Csn.MultiTenant;
using Csn.Retail.AppShellClient;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Csn.SimpleCqrs;
using Ingress.ServiceClient.Abstracts;
using Csn.Logging;
using ILogger = Csn.Logging.ILogger;

namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public class LoadGlobalSiteDataOnPageLoad<T> : IAsyncEventHandler<T> where T : IEvent
    {
        private const string CacheProfileNameAnonymous = "GlobalSite.Anonymous";
        private const string CacheProfileNameMember = "GlobalSite.Member";
        private const string CacheKey = "GlobalSiteProvider:Get:{0}:{1}";
        private const string ServiceName = "cmp-retail-appshell";
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly ISmartServiceClient _smartClient;
        private readonly ICacheStore _cacheStore;
        private readonly IUserContext _userContext;
        private readonly IContextStore<AppShellData> _contextStore;
        private readonly IAppShellClient _proxy;
        private readonly ILogger _logger;

        public LoadGlobalSiteDataOnPageLoad(ITenantProvider<TenantInfo> tenantProvider,
            ISmartServiceClient smartClient,
            ICacheStore cacheStore,
            IUserContext userContext,
            IContextStore<AppShellData> contextStore,
            IAppShellClient proxy,
            ILogger logger)
        {
            _tenantProvider = tenantProvider;
            _smartClient = smartClient;
            _cacheStore = cacheStore;
            _userContext = userContext;
            _contextStore = contextStore;
            _proxy = proxy;
            _logger = logger;
        }

        public async Task HandleAsync(T eEvent)
        {
            if (!(eEvent is IRequireGlobalSiteNav)) return;

            //var response = await _cacheStore
            //    .Profile(currentUserId.HasValue() ? CacheProfileNameMember : CacheProfileNameAnonymous)
            //        .FetchAsync(async () => await FetchFromApi())
            //        .CacheIf(x => x != null && x.TopNav.HasValue())
            //        .GetAsync(CacheKey.FormatWith(_tenantProvider.Current().Name, currentUserId));

            //_contextStore.Set(response);
            try
            {
                var response = _proxy.Get(new AppShellRequest
                {
                    CurrentUserId = _userContext.CurrentUserId?.ToString(),
                    SiteName = _tenantProvider.Current().Name
                });

                if (response.IsSucceed)
                {
                    _contextStore.Set(response.Data.Data);
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc, "Unable to load SiteNavigation v2");
            }
        }
    }

    public interface IRequireGlobalSiteNav { }
}