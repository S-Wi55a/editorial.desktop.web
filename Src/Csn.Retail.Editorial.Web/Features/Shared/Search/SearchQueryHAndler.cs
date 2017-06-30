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
    public class SearchQueryHandler : IAsyncQueryHandler<SearchQuery, object>
    {
        private readonly IEditorialRyvussApiProxy _ryvussProxy;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public SearchQueryHandler(IEditorialRyvussApiProxy ryvussProxy,
            ITenantProvider<TenantInfo> tenantProvider)
        {
            _ryvussProxy = ryvussProxy;
            _tenantProvider = tenantProvider;
        }

        public async Task<object> HandleAsync(SearchQuery query)
        {
            var result = await _ryvussProxy.GetAsync(new EditorialRyvussApiInput()
            {
                RyvussPredicates = string.IsNullOrEmpty(query.Query) ? $"Service.{_tenantProvider.Current().Name}." : query.Query
            });

            if (!result.IsSucceed) return null;

            return new RyvussResult()
            {
                Count = result.Data.Count,
                INav = new RyvussNav()
                {
                    Nodes = result.Data.INav.Nodes.Where(n => n.Name == "Type" || n.Name == "Make" || n.Name == "BodyType" || n.Name == "Year").ToList(),
                    BreadCrumbs = result.Data.INav.BreadCrumbs.ToList()
                },
                SearchResults = result.Data.SearchResults
            };
        }
    }
}