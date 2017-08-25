using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect
{
    [AutoBind]
    public class AspectQueryHandler : IAsyncQueryHandler<AspectQuery, NavResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;

        public AspectQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<NavResult> HandleAsync(AspectQuery query)
        {
            // TODO: Add validation filters for the query


            var result = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInputNew()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                IncludeCount = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { $"Retail({query.Aspect})", "ShowZero" }
            });

            var resultData = !result.IsSucceed ? null : result.Data;

            return resultData == null ? null : _mapper.Map<NavResult>(resultData);
        }
    }
}