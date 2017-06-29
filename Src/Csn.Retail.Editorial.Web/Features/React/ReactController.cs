using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Home;
using Csn.SimpleCqrs;
using System.Net;
using System.Web.Http;
using Csn.Retail.Editorial.Web.Features.Shared.Search;

namespace Csn.Retail.Editorial.Web.Features.React
{
    public class ReactController : Controller
    {

        [System.Web.Mvc.Route("editorial/react/")]
        // GET: React
        public ActionResult Index()
        {

            return View();
        }

        //[Route("editorial/react/listing")]
        //public ActionResult Listing()
        //{

        //    return View("~/Features/React/Views/Listing.cshtml");
        //}


        private readonly IQueryDispatcher _queryDispatcher;

        public ReactController(IQueryDispatcher queryDispatcher)
        {
            _queryDispatcher = queryDispatcher;
        }

        [System.Web.Mvc.Route("editorial/react/listing")]
        public async Task<ActionResult> Get([FromUri]string q = null)
        {
            var result = await _queryDispatcher.DispatchAsync<SearchQuery, object>(new SearchQuery()
            {
                Query = q
            });
            //System.Threading.Thread.Sleep(2000);
            return View("~/Features/React/Views/Listing.cshtml", result);
        }
    }
}
