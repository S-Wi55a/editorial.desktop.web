using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.AlsoConsider
{
    public class AlsoConsiderController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public AlsoConsiderController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/api/v1/also-consider")]
        public async Task<object> GetLatest(AlsoConsiderQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<AlsoConsiderQuery, AlsoConsiderDto>(query);

            return JsonConvert.SerializeObject(result);
        }
    }
}