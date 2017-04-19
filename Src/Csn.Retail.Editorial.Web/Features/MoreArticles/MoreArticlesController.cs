using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.SimpleCqrs;
using Newtonsoft.Json;

namespace Csn.Retail.Editorial.Web.Features.MoreArticles
{
    public class MoreArticlesController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public MoreArticlesController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }


        [HttpGet]
        [Route("editorial/api/v1/more-articles")]
        public async Task<object> GetLatest(MoreArticlesQuery query)
        {
            var result = await _queryDispatcher.DispatchAsync<MoreArticlesQuery, MoreArticlesDto>(query);

            return JsonConvert.SerializeObject(result);
        }
    }
}