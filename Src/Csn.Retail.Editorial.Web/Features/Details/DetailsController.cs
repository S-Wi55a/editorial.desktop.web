using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class DetailsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IDetailsRedirectLogger _redirectLogger;

        public DetailsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher, IDetailsRedirectLogger redirectLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _redirectLogger = redirectLogger;
        }

        [Route("editorial/details/{pageName:regex(^.*-\\d+/?$)}")]
        [RedirectAttributeFilter]
        // GET: Details
        public async Task<ActionResult> Index(ArticleIdentifier articleIdentifier, bool __preview = false)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
            {
                Id = articleIdentifier.Id,
                IsPreview = __preview
            });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            // redirect any article request with slug which does not match the published slug
            if (response.ArticleViewModel.Slug != articleIdentifier.PageName)
            {
                _redirectLogger.Log(HttpContext.Request.Url?.ToString());

                return new RedirectResult($"/editorial/details/{response.ArticleViewModel.Slug}/{Request.RequestContext.HttpContext.Request.Url?.Query}", true);
            }

            if (response.ArticleViewModel != null)
            {
                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return await (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404() : errorsController.ErrorGeneric());
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}