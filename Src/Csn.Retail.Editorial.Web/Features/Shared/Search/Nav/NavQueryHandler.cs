using System.Collections.Generic;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    [AutoBind]
    public class NavQueryHandler : IAsyncQueryHandler<NavQuery, NavResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;

        public NavQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<NavResult> HandleAsync(NavQuery query)
        {
            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput
            {
                Query = query.Query,
                IncludeCount = true,
                IncludeSearchResults = false,
                ControllerName = _tenantProvider.Current().SupportsSeoFriendlyListings ? $"seo-{_tenantProvider.Current().Name}" : "",
                ServiceProjectionName = _tenantProvider.Current().SupportsSeoFriendlyListings ? _tenantProvider.Current().Name : "",
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = new List<string> { "Seo", "Retail", "FacetSort", "ShowZero" }
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            if (resultData == null) return null;

            return new NavResult()
            {
                Count = resultData.Count,
                INav = _mapper.Map<Nav>(resultData.INav)
            };
        }
    }
}