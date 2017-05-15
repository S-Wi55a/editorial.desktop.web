using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.ApiProxy
{
    [AutoBind]
    public class ApiProxyQueryHandler : IAsyncQueryHandler<ApiProxyQuery, object>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;

        public ApiProxyQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }

        public async Task<object> HandleAsync(ApiProxyQuery query)
        {
            var result = await _editorialApiProxy.GetAsync(query.Uri);

            if (!result.IsSucceed)
            {
                return null;
            }

            return result.Data;
        }
    }
}