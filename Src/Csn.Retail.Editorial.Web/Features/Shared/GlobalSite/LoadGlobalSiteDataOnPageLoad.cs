using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Cars.Cache;
using Csn.Cars.Cache.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.ContextStores;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Csn.SimpleCqrs;
using Ingress.ServiceClient.Abstracts;

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
        private readonly IContextStore<GlobalSiteDataDto> _contextStore;

        public LoadGlobalSiteDataOnPageLoad(ITenantProvider<TenantInfo> tenantProvider,
            ISmartServiceClient smartClient,
            ICacheStore cacheStore,
            IUserContext userContext,
            IContextStore<GlobalSiteDataDto> contextStore)
        {
            _tenantProvider = tenantProvider;
            _smartClient = smartClient;
            _cacheStore = cacheStore;
            _userContext = userContext;
            _contextStore = contextStore;
        }

        public async Task HandleAsync(T eEvent)
        {
            if (!(eEvent is IRequireGlobalSiteNav)) return;

            var currentUserId = _userContext.CurrentUserId;

            var response = await _cacheStore
                .Profile(currentUserId.HasValue() ? CacheProfileNameMember : CacheProfileNameAnonymous)
                    .FetchAsync(async () => await FetchFromApi())
                    .CacheIf(x => x != null && x.TopNav.HasValue())
                    .GetAsync(CacheKey.FormatWith(_tenantProvider.Current().Name, currentUserId));

            _contextStore.Set(response);
        }

        private async Task<GlobalSiteDataDto> FetchFromApi()
        {
            var response = await _smartClient.Service(ServiceName)
                .Path(_tenantProvider.Current().SiteNavPath)
                .QueryString("memberId", _userContext.CurrentUserId.ToString())
                .GetAsync<GlobalSiteResponseDto>();

            return response?.Data?.Data;
        }
    }

    public interface IRequireGlobalSiteNav { }
}