using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.StockForSale
{
    [AutoBind]
    public class StockForSaleQueryHandler : IAsyncQueryHandler<StockForSaleQuery, StockForSaleDto>
    {
        private readonly IEditorialApiProxy _editorialApiProxy;


        public StockForSaleQueryHandler(IEditorialApiProxy editorialApiProxy)
        {
            _editorialApiProxy = editorialApiProxy;
        }


        public async Task<StockForSaleDto> HandleAsync(StockForSaleQuery query)
        {
            var result = await _editorialApiProxy.GetStockListingAsync(query);

            if (!result.Succeed)
            {
                return null;
            }

            return result.Result;
        }
    }
}