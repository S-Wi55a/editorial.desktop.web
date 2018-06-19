using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.ActionAttributes;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
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
        [Route("editorial/{*slug:regex((features|riding-advice|tips|tow-tests|motoracing|engine-reviews|reviews|news|advice|videos|products)(/.*-\\d+/?$))}")]
        [RedirectToNewVersion]
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

            if (!string.IsNullOrEmpty(response.RedirectUrl))
            {
                return PermanentRedirect($"{response.RedirectUrl}{Request.RequestContext.HttpContext.Request.Url?.Query}");
            }

            if (response.ArticleViewModel != null)
            {
                // redirect any article request with slug which does not match the published slug
                if (response.ArticleViewModel.Slug != articleIdentifier.PageName)
                {
                    return PermanentRedirect($"/editorial/details/{response.ArticleViewModel.Slug}/{Request.RequestContext.HttpContext.Request.Url?.Query}");
                }

                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return await (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404() : errorsController.ErrorGeneric());
        }

        private ActionResult PermanentRedirect(string redirectUrl)
        {
            // log the url which is being redirected
            _redirectLogger.Log(HttpContext.Request.Url?.ToString());

            return new RedirectResult(redirectUrl, true);
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}