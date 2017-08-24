﻿using System.Threading.Tasks;
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
        public async Task<IHttpActionResult> GetNav([FromUri]string q = null)
        {
            var result = await _queryDispatcher.DispatchAsync<NavQuery, NavResult>(new NavQuery()
            {
                Query = q
            });

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav/aspect")]
        public async Task<IHttpActionResult> GetAspect([FromUri]string q = null)
        {
            // to be replaced with aspect query handler
            var result = await _queryDispatcher.DispatchAsync<AspectQuery, NavResult>(new AspectQuery()
            {
                Query = q
            });

            if (result != null) return Ok(result);

            return NotFound();
        }

        [HttpGet]
        [Route("editorial/api/v1/search/nav/aspect/refinements")]
        public async Task<IHttpActionResult> GetAspectRefinements([FromUri]string q = null)
        {
            // to be replaced with refinements query handler
            var result = await _queryDispatcher.DispatchAsync<RefinementsQuery, NavResult>(new RefinementsQuery()
            {
                Query = q
            });

            if (result != null) return Ok(result);

            return NotFound();
        }
    }
}