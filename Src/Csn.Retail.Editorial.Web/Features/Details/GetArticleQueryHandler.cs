using System.Net;
using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.ContextStores;
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
        private readonly IPageContextStore _pageContextStore;

        public GetArticleQueryHandler(IEditorialApiProxy editorialApiProxy, IMapper mapper, ITenantProvider<TenantInfo> tenantProvider, IPageContextStore pageContextStore)
        {
            _editorialApiProxy = editorialApiProxy;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
            _pageContextStore = pageContextStore;
        }

        public async Task<GetArticleResponse> HandleAsync(GetArticleQuery query)
        {
            var result = await _editorialApiProxy.GetArticleAsync(new EditorialApiInput()
            {
                ServiceName = _tenantProvider.Current().RyvusServiceProjection,
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

            if (!string.IsNullOrEmpty(result.Data.RedirectUrl))
            {
                return new GetArticleResponse()
                {
                    HttpStatusCode = HttpStatusCode.MovedPermanently,
                    RedirectUrl = result.Data.RedirectUrl
                };
            }

            var articleViewModel = _mapper.Map<ArticleViewModel>(result.Data);
            
            var detailsPageContext = new DetailsPageContext
            {
                Items = articleViewModel.Items,
                Lifestyles = articleViewModel.Lifestyles,
                Categories = articleViewModel.Categories,
                Keywords = articleViewModel.Keywords,
                ArticleType = articleViewModel.ArticleType,
                ArticleTypes = articleViewModel.ArticleTypes,
            };

            _pageContextStore.Set(detailsPageContext);

            // only the article to be SEO indexed if not in preview mode
            articleViewModel.SeoData.AllowSeoIndexing = articleViewModel.SeoData.AllowSeoIndexing && !query.IsPreview;

            return new GetArticleResponse()
            {
                ArticleViewModel = articleViewModel
            };
        }
    }
}