using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.LatestArticles
{
    [AutoBind]
    public class LatestArticlesQueryHandler : IAsyncQueryHandler<LatestArticlesQuery, LatestArticlesDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;
        private readonly IMapper _mapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;


        public LatestArticlesQueryHandler(IEditorialApiProxy editorialApiProxy, IMapper mapper,
            ITenantProvider<TenantInfo> tenantProvider)
        {
            _editorialApiProxy = editorialApiProxy;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
        }


        public async Task<LatestArticlesDto> HandleAsync(LatestArticlesQuery query)
        {
            // Add the service name
            query.ServiceName = _tenantProvider.Current().Name;

            var result = await _editorialApiProxy.GetLatestArticlesAsync(query);

            if (!result.Succeed)
            {
                return null;
            }

            return result.Result;
        }
    }
}