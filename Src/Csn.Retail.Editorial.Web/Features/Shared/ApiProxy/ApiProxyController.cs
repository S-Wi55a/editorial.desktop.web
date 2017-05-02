using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.Shared.ApiProxy
{
    public class ApiProxyController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public ApiProxyController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [HttpGet]
        [Route("editorial/api/v1/proxy")]
        public async Task<object> Get(ApiProxyQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<ApiProxyQuery, object>(query);

            return JsonConvert.SerializeObject(result);
        }
    }
}