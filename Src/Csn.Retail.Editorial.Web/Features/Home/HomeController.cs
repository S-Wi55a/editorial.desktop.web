using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Home
{
    public class HomeController : Controller
    {
        private readonly IEventDispatcher _eventDispatcher;

        public HomeController(IEventDispatcher eventDispatcher)
        {
            _eventDispatcher = eventDispatcher;
        }

        // GET: Home
        public async Task<ActionResult> Index()
        {
            await _eventDispatcher.DispatchAsync(new HomePageRequestEvent());

            return View();
        }
    }

    public class HomePageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}