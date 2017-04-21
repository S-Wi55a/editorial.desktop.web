using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Spec
{
    [AutoBind]
    public class SpecQueryHandler : IAsyncQueryHandler<SpecQuery, object>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;

        public SpecQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }

        public async Task<object> HandleAsync(SpecQuery query)
        {
            var result = await _editorialApiProxy.GetSpecAsync(query);

            if (!result.IsSucceed)
            {
                return null;
            }

            return result.Data;
        }
    }
}