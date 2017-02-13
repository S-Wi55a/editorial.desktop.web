using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.SiteNav.Models;
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

        [ChildActionOnly]
        public ActionResult TopNav(SiteNavQuery query)
        {
            var topNav = _queryDispatcher.Dispatch<SiteNavQuery, SiteNavViewModel>(query);

            return PartialView("TopNav", topNav);
        }


        public ActionResult FooterNav(SiteNavQuery query)
        {
            var footerNav = _queryDispatcher.Dispatch<SiteNavQuery, SiteNavViewModel>(query);

            return PartialView("FooterNav", footerNav);
        }
    }
}