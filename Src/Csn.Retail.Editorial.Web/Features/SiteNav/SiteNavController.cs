using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.SiteNav
{
    public class SiteNavController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;

        public SiteNavController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        public async Task<ActionResult> TopNav(SiteNavQuery query)
        {
            var topNav = await _queryDispatcher.DispatchAsync<SiteNavQuery, SiteNavViewModel>(query);
            return null;
        }

    }
}