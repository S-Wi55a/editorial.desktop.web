using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MoreArticles
{
    [AutoBind]
    public class MoreArticlesQueryHandler : IAsyncQueryHandler<MoreArticlesQuery, MoreArticlesDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;


        public MoreArticlesQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }


        public async Task<MoreArticlesDto> HandleAsync(MoreArticlesQuery query)
        {
            var result = await _editorialApiProxy.GetLatestArticlesAsync(query);

            if (!result.IsSucceed)
            {
                return null;
            }

            return result.Data;
        }
    }
}