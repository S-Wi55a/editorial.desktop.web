using System.Threading.Tasks;
using Bolt.Common.Extensions;
using Csn.Cars.Cache;
using Csn.Cars.Cache.Extensions;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.SiteNavApi;
using Csn.Retail.Editorial.Web.Features.SiteNav.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.Retail.Editorial.Web.Infrastructure.UserContext;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.SiteNav
{
    [AutoBind]
    public class SiteNavQueryHandler : IQueryHandler<SiteNavQuery, SiteNavViewModel>
    {
        private readonly ICacheStore _cacheStore;
        private readonly IMapper _mapper;
        private readonly ISiteNavApiProxy _siteNavApiProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IUserContext _userContext;

        private const string CacheProfileNameMember = "GlobalSite.Member";
        private const string CacheProfileNameAnonymous = "GlobalSite.Anonymous";
        private const string CacheKey = "GlobalSiteProvider:Get:{0}:{1}";

        public SiteNavQueryHandler(ICacheStore cacheStore, IMapper mapper, ISiteNavApiProxy siteNavApiProxy,
            ITenantProvider<TenantInfo> tenantProvider, IUserContext userContext)
        {
            _cacheStore = cacheStore;
            _mapper = mapper;
            _siteNavApiProxy = siteNavApiProxy;
            _tenantProvider = tenantProvider;
            _userContext = userContext;
        }


        public SiteNavViewModel Handle(SiteNavQuery query)
        {
            var currentUserId = _userContext.CurrentUserId;
            var site = _tenantProvider.Current().Name;

            if (query.RefreshCache)
            {
                _cacheStore.Remove(CacheKey.FormatWith(site, currentUserId));
            }

            return _cacheStore.Profile(currentUserId.HasValue() ? CacheProfileNameMember : CacheProfileNameAnonymous).Fetch(() => FetchFromApi(site)).CacheIf(x => x != null).Get(CacheKey.FormatWith(_tenantProvider.Current().Name, currentUserId));
        }


        private SiteNavViewModel FetchFromApi(string site)
        {
            var result = _siteNavApiProxy.GetSiteNav(site);

            if (!result.Succeed)
            {
                return null;
            }

            return _mapper.Map<SiteNavViewModel>(result.Result.Data);
        }
    }
}