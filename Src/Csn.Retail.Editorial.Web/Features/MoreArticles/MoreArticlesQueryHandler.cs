using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MoreArticles
{
    [AutoBind]
    public class MoreArticlesQueryHandler : IAsyncQueryHandler<MoreArticlesQuery, MoreArticlesDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;
        private readonly IMapper _mapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;


        public MoreArticlesQueryHandler(IEditorialApiProxy editorialApiProxy, IMapper mapper,
            ITenantProvider<TenantInfo> tenantProvider)
        {
            _editorialApiProxy = editorialApiProxy;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
        }


        public async Task<MoreArticlesDto> HandleAsync(MoreArticlesQuery query)
        {
            var result = await _editorialApiProxy.GetLatestArticlesAsync(query);

            if (!result.Succeed)
            {
                return null;
            }

            return result.Result;
        }
    }
}