using System;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.AppShellClient;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Csn.SimpleCqrs;
using Csn.Logging;
using ILogger = Csn.Logging.ILogger;

namespace Csn.Retail.Editorial.Web.Features.Shared.GlobalSite
{
    public class LoadGlobalSiteDataOnPageLoad<T> : IAsyncEventHandler<T> where T : IEvent
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IUserContext _userContext;
        private readonly IContextStore<AppShellData> _contextStore;
        private readonly IAppShellClient _proxy;
        private readonly ILogger _logger;

        public LoadGlobalSiteDataOnPageLoad(ITenantProvider<TenantInfo> tenantProvider,
            IUserContext userContext,
            IContextStore<AppShellData> contextStore,
            IAppShellClient proxy,
            ILogger logger)
        {
            _tenantProvider = tenantProvider;
            _userContext = userContext;
            _contextStore = contextStore;
            _proxy = proxy;
            _logger = logger;
        }

        public async Task HandleAsync(T eEvent)
        {
            if (!(eEvent is IRequireGlobalSiteNav)) return;

            try
            {
                var response = _proxy.Get(new AppShellRequest
                {
                    CurrentUserId = _userContext.CurrentUserId?.ToString(),
                    SiteName = !_tenantProvider.Current().AvailableOnRedbook ? _tenantProvider.Current().Name : "redbook"
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