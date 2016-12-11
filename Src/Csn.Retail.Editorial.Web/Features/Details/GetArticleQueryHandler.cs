using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [AutoBind]
    public class GetArticleQueryHandler : IAsyncQueryHandler<GetArticleQuery, ArticleViewModel>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;

        public GetArticleQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }

        public async Task<ArticleViewModel> HandleAsync(GetArticleQuery query)
        {
            var result = await _editorialApiProxy.GetArticleAsync(new EditorialApiInput()
            {
                ServiceName = "carsales",
                ViewType = "desktop",
                Id = query.Id
            });

            if (!result.Succeed)
            {
                return null;
            }

            var detailsDto = result.Result;

            return new ArticleViewModel()
            {
                ArticleTemplateType = detailsDto.ArticleTemplateType,
                Headline = detailsDto.Headline,
                HeroSection = detailsDto.HeroSection
            };
        }
    }
}