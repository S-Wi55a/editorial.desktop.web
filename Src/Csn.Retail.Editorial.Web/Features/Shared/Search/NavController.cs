using System.Threading.Tasks;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search
{
    public class NavController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public NavController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public async Task<IHttpActionResult> GetNav([FromUri]NavQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<NavQuery, NavResult>(query ?? new NavQuery());

            if (result != null) return Ok(result);

            return NotFound();
        }

        public async Task<IHttpActionResult> GetRefinements([FromUri]RefinementsQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<RefinementsQuery, RefinementResult>(query);

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}
