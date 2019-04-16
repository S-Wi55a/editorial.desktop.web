using System.Net;
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
    public class PreviewQueryHandler : IAsyncQueryHandler<PreviewQuery, GetArticleResponse>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;
        private readonly IMapper _mapper;
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public PreviewQueryHandler(IEditorialApiProxy editorialApiProxy, IMapper mapper, ITenantProvider<TenantInfo> tenantProvider)
        {
            _editorialApiProxy = editorialApiProxy;
            _mapper = mapper;
            _tenantProvider = tenantProvider;
        }

        public async Task<GetArticleResponse> HandleAsync(PreviewQuery query)
        {
            var result = await _editorialApiProxy.GetPreviewAsync(new EditorialApiInput()
            {
                ServiceName = _tenantProvider.Current().TenantName,
                Id = query.Id
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

            // Remove unwanted
            articleViewModel.DisqusData = null;

            return new GetArticleResponse()
            {
                ArticleViewModel = articleViewModel,
                DetailsPageUrlPath = result.Data.DetailsPageUrlPath,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}