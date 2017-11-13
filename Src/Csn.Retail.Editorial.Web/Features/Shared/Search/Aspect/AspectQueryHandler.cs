using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect
{
    [AutoBind]
    public class AspectQueryHandler : IAsyncQueryHandler<AspectQuery, AspectResult>
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

        public async Task<AspectResult> HandleAsync(AspectQuery query)
        {
            var ryvussResult = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                IncludeCount = true,
                NavigationName = _tenantProvider.Current().SiteNavName,
                PostProcessors = new List<string> { "Retail", "FacetSort", $"RetailAspect({query.Aspect})", "ShowZero" }
            });

            var resultData = !ryvussResult.IsSucceed ? null : ryvussResult.Data;

            if (resultData?.INav?.Nodes == null || !resultData.INav.Nodes.Any()) return null;

            var result = _mapper.Map<AspectResult>(resultData.INav.Nodes.First(), opt => { opt.Items["sortOrder"] = query.SortOrder; });

            result.Count = resultData.Count;

            return result;
        }
    }
}