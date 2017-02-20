using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.LatestArticles
{
    public class LatestArticlesController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public LatestArticlesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/latest-articles/{id}")]
        public async Task<ActionResult> GetLatest(string id, LatestArticlesQuery query)
        {
            var latestQuery = new LatestArticlesQuery
            {
                Id = id,
                Limit = query.Limit,
                Offset = query.Offset
            };

            var result = await _queryDispatcher.DispatchAsync<LatestArticlesQuery, LatestArticlesDto>(latestQuery);

            return PartialView("LatestArticlesView", result);
        }
    }
}