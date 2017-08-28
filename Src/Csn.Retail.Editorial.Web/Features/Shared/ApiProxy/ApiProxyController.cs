using System.Threading.Tasks;
using System.Web.Http;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.ApiProxy
{
    public class ApiProxyController : ApiController
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ApiProxyController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [Route("editorial/api/v1/proxy")]
        public async Task<IHttpActionResult> Get([FromUri]ApiProxyQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<ApiProxyQuery, object>(query);

            return Ok(result);
        }
    }
}