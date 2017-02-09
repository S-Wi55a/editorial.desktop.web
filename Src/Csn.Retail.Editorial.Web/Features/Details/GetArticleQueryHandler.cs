using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [AutoBind]
    public class GetArticleQueryHandler : IAsyncQueryHandler<GetArticleQuery, ArticleViewModel>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;
        private readonly IMapper _mapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public GetArticleQueryHandler(IEditorialApiProxy editorialApiProxy, IMapper mapper, ITenantProvider<TenantInfo> tenantProvider)
        {
            _editorialApiProxy = editorialApiProxy;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
        }

        public async Task<ArticleViewModel> HandleAsync(GetArticleQuery query)
        {
            var result = await _editorialApiProxy.GetArticleAsync(new EditorialApiInput()
            {
                ServiceName = _tenantProvider.Current().Name,
                ViewType = "desktop",
                Id = query.Id
            });

            if (!result.Succeed)
            {
                return null;
            }

            return _mapper.Map<ArticleViewModel>(result.Result);
        }
    }
}