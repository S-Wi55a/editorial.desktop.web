using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Home;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.React
{
    public class ReactController : Controller
    {

        [Route("editorial/react/")]
        // GET: React
        public ActionResult Index()
        {

            return View();
        }

        [Route("editorial/react/listing")]
        public ActionResult Listing()
        {

            return View("~/Features/React/Views/Listing.cshtml");
        }
    }
}
