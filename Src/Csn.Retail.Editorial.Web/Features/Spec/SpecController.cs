using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Spec
{
    public class SpecController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public SpecController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/api/v1/spec")]
        public async Task<object> GetLatest(ApiQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<ApiQuery, object>(query);

            return JsonConvert.SerializeObject(result);
        }

        [HttpGet]
        [Route("editorial/api/v1/retail-editorial-proxy")]
        public async Task<object> Get(ApiQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<ApiQuery, object>(query);

            return JsonConvert.SerializeObject(result);
        }
    }
}