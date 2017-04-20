using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.AlsoConsider
{
    [AutoBind]
    public class AlsoConsiderQueryHandler : IAsyncQueryHandler<AlsoConsiderQuery, AlsoConsiderDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;


        public AlsoConsiderQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }

        public async Task<AlsoConsiderDto> HandleAsync(AlsoConsiderQuery query)
        {
            var result = await _editorialApiProxy.GetAlsoConsiderAsync(query);

            if (!result.IsSucceed)
            {
                return null;
            }

            return result.Data;
        }
    }
}