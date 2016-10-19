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
            var result = await _editorialApiProxy.GetAsync<ArticleDto>(new EditorialApiInput()
            {
                ServiceName = "carsales",
                ViewType = "desktop",
                Id = "ed-itm-51707"
            });

            if (!result.Succeed)
            {
                return null;
            }

            return new ArticleViewModel()
            {
                Headline = result.Result.Headline
            };
        }

        public class ArticleDto
        {
            public string ArticleType { get; set; }
            public string Headline { get; set; }
            public string Subheading { get; set; }
            public string Summary { get; set; }
        }
    }
}