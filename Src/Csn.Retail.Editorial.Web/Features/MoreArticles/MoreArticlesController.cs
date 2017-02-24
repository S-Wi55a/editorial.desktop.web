using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.MoreArticles
{
    public class MoreArticlesController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public MoreArticlesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/api/more-articles")]
        public async Task<ActionResult> GetLatest(MoreArticlesQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<MoreArticlesQuery, MoreArticlesDto>(query);

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}


