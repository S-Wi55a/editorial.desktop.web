using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.StockForSale
{
    public class StockForSaleController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public StockForSaleController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/api/v1/stock-listing")]
        public async Task<ActionResult> GetLatest(StockForSaleQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<StockForSaleQuery, StockForSaleDto>(query);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}


