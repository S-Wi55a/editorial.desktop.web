using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
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
        [Route("editorial/api/v1/search")]
        public async Task<IHttpActionResult> Get([FromUri]string q = null)
        {
            var result = await _queryDispatcher.DispatchAsync<SearchQuery, object>(new SearchQuery()
            {
                Query = q
            });

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}
