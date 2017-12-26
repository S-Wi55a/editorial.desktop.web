using System.Threading.Tasks;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Landing.Carousel;
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
            var result = await _queryDispatcher.DispatchAsync<NavQuery, NavResult>(query ?? new NavQuery());

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav/refinements")]
        public async Task<IHttpActionResult> GetRefinements([FromUri]RefinementsQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<RefinementsQuery, RefinementResult>(query);

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/carousel/")]
        public async Task<IHttpActionResult> GetCarousel([FromUri]CarouselQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<CarouselQuery, CarouselQueryResponse>(query);

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}
