using System.Linq;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search
{
    [AutoBind]
    public class SearchQueryHandler : IAsyncQueryHandler<SearchQuery, RyvussResult>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SearchQueryHandler(IEditorialRyvussApiProxy ryvussProxy,
            ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
        }

        public async Task<RyvussResult> HandleAsync(SearchQuery query)
        {
            //var input = new EditorialRyvussNavInput()
            //{
            //    Query = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
            //    Limit = query.Limit == 0 ? 10 : query.Limit,
            //    Offset = query.Offset
            //};

            //var result = await _ryvussProxy.GetAsync<RyvussNavResult>(input);

            var result = await _ryvussProxy.GetAsync(new EditorialRyvussApiInput()
            {
                RyvussPredicates = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query,
                Limit = query.Limit == 0 ? 10 : query.Limit,
                Offset = query.Offset
            });

            if (!result.IsSucceed) return null;

            return result.Data;
        }
    }

    public class RyvussNavResult
    {
        
    }
}