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
            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                Limit = 10,
                Offset = 0,
                SortOrder = "Latest",
                IncludeCount = true,
                IncludeSearchResults = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { "Retail", "ShowZero"}
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            return resultData == null ? null : _mapper.Map<NavResult>(resultData);
        }
    }
}