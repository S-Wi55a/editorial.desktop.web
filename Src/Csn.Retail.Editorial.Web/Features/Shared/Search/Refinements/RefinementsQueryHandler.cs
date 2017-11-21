using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    [AutoBind]
    public class RefinementsQueryHandler : IAsyncQueryHandler<RefinementsQuery, RefinementResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public RefinementsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
        }

        public async Task<RefinementResult> HandleAsync(RefinementsQuery query)
        {
            // TODO: Add validation filters for the query


            var ryvussResult = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                IncludeCount = true,
                NavigationName = _tenantProvider.Current().RyvusNavName,
                PostProcessors = new List<string> { "FacetSort", $"RetailAspect({query.Aspect})", $"RetailAspectRefinements({query.AspectRefinement},{query.ParentExpression})", "ShowZero" }
            });

            var resultData = !ryvussResult.IsSucceed ? null : ryvussResult.Data;

            if (resultData?.INav?.Nodes == null || !resultData.INav.Nodes.Any()) return null;

            var firstNode = resultData.INav.Nodes.First();

            var result = AutoMapper.Mapper.Map<RefinementResult>(firstNode, opt => {
                opt.Items["sortOrder"] = query.SortOrder;
            });

            result.Count = resultData.Count;

            return result;
        }
    }
}