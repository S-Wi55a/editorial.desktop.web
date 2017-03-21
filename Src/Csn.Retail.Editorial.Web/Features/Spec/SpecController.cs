using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
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
        public async Task<object> GetLatest(SpecQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<SpecQuery, SpecDto>(query);

            return JsonConvert.SerializeObject(result);
        }
    }
}
