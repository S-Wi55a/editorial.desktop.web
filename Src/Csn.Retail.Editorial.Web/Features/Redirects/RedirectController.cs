using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.Wrappers;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public class RedirectController : Controller
    {
        private readonly IRedirectService _redirectService;
        private readonly IRedirectLogger _redirectLogger;
        private readonly IUrlRouteHelper _urlRouteHelper;

        public RedirectController(IRedirectService redirectService, IRedirectLogger redirectLogger, IUrlRouteHelper urlRouteHelper)
        {
            _redirectService = redirectService;
            _redirectLogger = redirectLogger;
            _urlRouteHelper = urlRouteHelper;
        }

        public ActionResult Redirect()
        {
            _redirectLogger.Log(HttpContext.Request.Url?.AbsolutePath);

            var redirect = _redirectService.GetRedirect();

            if (redirect != null)
            {
                return redirect;
            }

            return new RedirectResult(_urlRouteHelper.HttpRouteUrl(RouteNames.Mvc.LandingHome, null), true);
        }
    }
}