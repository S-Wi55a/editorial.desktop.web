using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;

namespace Csn.Retail.Editorial.Web.Features.Redirects
{
    public class RedirectController : Controller
    {
        private readonly IRedirectService _redirectService;
        private readonly IRedirectLogger _redirectLogger;

        public RedirectController(IRedirectService redirectService, IRedirectLogger redirectLogger)
        {
            _redirectService = redirectService;
            _redirectLogger = redirectLogger;
        }

        public ActionResult Redirect()
        {
            var redirect = _redirectService.GetRedirect();
            if (redirect?.RedirectResult != null)
            {
                _redirectLogger.Log(redirect, HttpContext.Request.Url?.AbsolutePath);
                return redirect.RedirectResult;
            }
            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);
            return errorsController.Error404Child();
        }
    }
}