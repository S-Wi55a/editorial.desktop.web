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

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    [AutoBind]
    public class RefinementsQueryHandler : IAsyncQueryHandler<RefinementsQuery, RefinementResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;
        private readonly IMapper _mapper;

        public RefinementsQueryHandler(IEditorialRyvussApiProxy ryvussProxy, ITenantProvider<TenantInfo> tenantProvider, IMapper mapper)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
            _mapper = mapper;
        }

        public async Task<RefinementResult> HandleAsync(RefinementsQuery query)
        {
            // TODO: Add validation filters for the query


            var ryvussResult = await _ryvussProxy.GetAsync<RyvussNavResultDto>(new EditorialRyvussInput()
            {
                Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                IncludeCount = true,
                NavigationName = "RetailNav",
                PostProcessors = new List<string> { $"Retail({query.Aspect},{query.ParentExpression})", "ShowZero" }
            });

            var resultData = !ryvussResult.IsSucceed ? null : ryvussResult.Data;

            if (resultData?.INav?.Nodes == null || !resultData.INav.Nodes.Any()) return null;

            var firstNode = resultData.INav.Nodes.First();

            var result = _mapper.Map<RefinementResult>(firstNode);

            result.Count = resultData.Count;

            return result;
        }
    }
}