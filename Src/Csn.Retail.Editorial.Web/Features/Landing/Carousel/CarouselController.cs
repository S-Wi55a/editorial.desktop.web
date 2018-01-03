using System.Threading.Tasks;
using System.Web.Http;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing.Carousel
{
    public class CarouselController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public CarouselController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
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