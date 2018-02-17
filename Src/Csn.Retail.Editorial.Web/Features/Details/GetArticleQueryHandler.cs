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
    public class GetArticleQueryHandler : IAsyncQueryHandler<GetArticleQuery, GetArticleResponse>
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

        public async Task<GetArticleResponse> HandleAsync(GetArticleQuery query)
        {
            var result = await _editorialApiProxy.GetArticleAsync(new EditorialApiInput()
            {
                ServiceName = _tenantProvider.Current().TenantName,
                ViewType = "desktop",
                Id = query.Id,
                IsPreview = query.IsPreview
            });

            if (!result.IsSucceed)
            {
                return new GetArticleResponse()
                {
                    HttpStatusCode = result.HttpStatusCode
                };
            }

            var articleViewModel = _mapper.Map<ArticleViewModel>(result.Data);

            // only the article to be SEO indexed if not in preview mode
            articleViewModel.SeoData.AllowSeoIndexing = articleViewModel.SeoData.AllowSeoIndexing && !query.IsPreview;

            return new GetArticleResponse()
            {
                ArticleViewModel = articleViewModel
            };
        }
    }
}