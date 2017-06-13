using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Errors
{
    public class ErrorsController : Controller
    {
        private readonly IUrlNotFoundLogger _urlNotFoundLogger;

        public ErrorsController(IUrlNotFoundLogger urlNotFoundLogger)
        {
            _urlNotFoundLogger = urlNotFoundLogger;
        }

        public async Task<ActionResult> Error404()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            _urlNotFoundLogger.Log(HttpContext.Request.Url.ToString());

            return View("~/Features/Errors/Views/Error404.cshtml");
        }

        public async Task<ActionResult> ErrorGeneric()
        {
            Response.StatusCode = (int)HttpStatusCode.NotFound;
            Response.TrySkipIisCustomErrors = true;

            _urlNotFoundLogger.Log(HttpContext.Request.Url.ToString());

            return View("~/Features/Errors/Views/ErrorGeneric.cshtml");
        }
    }
}