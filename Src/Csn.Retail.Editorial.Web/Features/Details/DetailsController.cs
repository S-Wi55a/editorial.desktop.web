using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class DetailsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IArticleNotFoundLogger _logger;

        public DetailsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher, IArticleNotFoundLogger logger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _logger = logger;
        }

        [Route("editorial/details/{pageName:regex(^.*-\\d+/?$)}")]
        [RedirectAttributeFilter]
        // GET: Details
        public async Task<ActionResult> Index(ArticleIdentifier articleIdentifier)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());
            
            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
                {
                    Id = articleIdentifier.Id
                });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response.ArticleViewModel != null)
            {
                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var httpStatusCode = HttpStatusCode.InternalServerError;

            if (response.HttpStatusCode == HttpStatusCode.NotFound)
            {
                _logger.Log(HttpContext.Request.Url.ToString());
                httpStatusCode = HttpStatusCode.NotFound;
            }

            Response.StatusCode = (int)httpStatusCode;
            Response.TrySkipIisCustomErrors = true;

            return View("~/Features/Errors/Views/ErrorTemplate.cshtml", new ErrorViewModel() { HttpStatusCode = httpStatusCode });
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}