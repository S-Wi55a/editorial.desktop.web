using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Details.Loggers;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class DetailsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly IDetailsRedirectLogger _redirectLogger;

        public DetailsController(IQueryDispatcher queryDispatcher, 
                                IEventDispatcher eventDispatcher, 
                                IDetailsRedirectLogger redirectLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _redirectLogger = redirectLogger;
        }

        [RedirectAttributeFilter]
        // ReSharper disable once InconsistentNaming
        public Task<ActionResult> IndexDetailsV1(ArticleIdentifierV1 articleIdentifier, bool __preview = false)
        {
            return Details(articleIdentifier.NetworkId, __preview);
        }

        [RedirectAttributeFilter]
        // ReSharper disable once InconsistentNaming
        public Task<ActionResult> IndexDetailsV2(ArticleIdentifierV2 articleIdentifier, bool __preview = false)
        {
            return Details(articleIdentifier.NetworkId, __preview);
        }

        private async Task<ActionResult> Details(string networkId, bool isPreview)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
            {
                Id = networkId,
                IsPreview = isPreview
            });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (!string.IsNullOrEmpty(response.RedirectUrl))
            {
                return PermanentRedirect($"{response.RedirectUrl}{Request.Url?.Query}");
            }

            if (response.ArticleViewModel != null)
            {
                // redirect any article request where the path does not match the path required by DetailsPageUrlPath
                if (Request.Url != null && !Request.Url.AbsolutePathUnescaped().Equals(response.DetailsPageUrlPath, StringComparison.InvariantCultureIgnoreCase))
                {
                    return PermanentRedirect($"{response.DetailsPageUrlPath}{Request.Url?.Query}");
                }

                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404Child() : errorsController.ErrorGenericChild();
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