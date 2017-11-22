using System.Threading.Tasks;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search
{
    
    public class SearchController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public SearchController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav")]
        public async Task<IHttpActionResult> GetNav([FromUri]NavQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<NavQuery, NavResult>(query);

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav/aspects/{aspect}/refinements")]
        public async Task<IHttpActionResult> GetAspectRefinements(string aspect, [FromUri]string refinementAspect, [FromUri]string parentExpression, [FromUri]string sortOrder = null, [FromUri]string q = null)
        {
            // to be replaced with refinements query handler
            var result = await _queryDispatcher.DispatchAsync<RefinementsQuery, RefinementResult>(new RefinementsQuery()
            {
                Query = q,
                Aspect = aspect,
                AspectRefinement = refinementAspect,
                ParentExpression = parentExpression,
                SortOrder = sortOrder
            });

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav/aspects/{aspect}")]
        public async Task<IHttpActionResult> GetAspect(string aspect, [FromUri]string sortOrder = null, [FromUri]string q = null)
        {
            // to be replaced with aspect query handler
            var result = await _queryDispatcher.DispatchAsync<AspectQuery, AspectResult>(new AspectQuery()
            {
                Query = q,
                Aspect = aspect,
                SortOrder = sortOrder
            });

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}
