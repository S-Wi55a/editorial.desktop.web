using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
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

            if (response.HttpStatusCode == HttpStatusCode.NotFound)
            {
                _logger.Log(HttpContext.Request.Url.ToString());
                // log the not found
                return new HttpNotFoundResult();
            }

            throw new HttpException(500, "Unable to retrieve article details");
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}