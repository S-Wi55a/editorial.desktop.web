using System.Threading.Tasks;
using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Mappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Spec
{
    [AutoBind]
    public class SpecQueryHandler : IAsyncQueryHandler<SpecQuery, SpecDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;


        public SpecQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }


        public async Task<SpecDto> HandleAsync(SpecQuery query)
        {
            var result = await _editorialApiProxy.GetSpecAsync(query);

            if (!result.Succeed)
            {
                return null;
            }

            return result.Result;
        }
    }
}


