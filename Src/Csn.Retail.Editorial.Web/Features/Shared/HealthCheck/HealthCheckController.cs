using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Infrastructure.HealthChecks;

namespace Csn.Retail.Editorial.Web.Features.Shared.HealthCheck
{
    public class HealthCheckController : Controller
    {
        private readonly IHealthReporter _reporter;

        public HealthCheckController(IHealthReporter reporter)
        {
            _reporter = reporter;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("healthcheck")]
        public async Task<ActionResult> HealthCheck()
        {
            return Json(await _reporter.ReportAsync(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("ping")]
        public ActionResult Ping()
        {
            return Content("pong");
        }
    }
}