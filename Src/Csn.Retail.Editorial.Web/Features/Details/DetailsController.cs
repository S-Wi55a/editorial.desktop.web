using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
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
        private readonly ILegacyDetailsRedirectLogger _legacyDetailsRedirectLogger;

        public DetailsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher, IDetailsRedirectLogger redirectLogger, ILegacyDetailsRedirectLogger legacyDetailsRedirectLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _redirectLogger = redirectLogger;
            _legacyDetailsRedirectLogger = legacyDetailsRedirectLogger;
        }

        [RedirectAttributeFilter]
        // ReSharper disable once InconsistentNaming
        public async Task<ActionResult> IndexDetailsV1(ArticleIdentifierV1 articleIdentifier, bool __preview = false)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
            {
                Id = articleIdentifier.NetworkId,
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
                if (!articleIdentifier.Slug.Trim('/').Equals(response.ArticleViewModel.Slug, StringComparison.InvariantCultureIgnoreCase))
                {
                    return PermanentRedirect($"/editorial/details/{response.ArticleViewModel.Slug.Trim('/')}/{Request.RequestContext.HttpContext.Request.Url?.Query}");
                }

                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404Child() : errorsController.ErrorGenericChild());
        }

        public async Task<ActionResult> IndexDetailsV2(ArticleIdentifierV2 articleIdentifier, bool __preview = false)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetArticleQuery, GetArticleResponse>(new GetArticleQuery()
            {
                Id = articleIdentifier.NetworkId,
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
                // TODO: reinstate this logic once we have the new DetailsPageUrl data in data service and ryvuss
                // redirect any article request with slug which does not match the published slug
                //if (!articleIdentifier.UrlPath.Equals(response.ArticleViewModel.Slug, StringComparison.InvariantCultureIgnoreCase))
                //{
                //    return PermanentRedirect($"{response.ArticleViewModel.Slug.Trim('/')}/{Request.RequestContext.HttpContext.Request.Url?.Query}");
                //}

                return View("DefaultTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404Child() : errorsController.ErrorGenericChild());
        }

        public ActionResult RedirectLegacyUrl()
        {
            var rawSlug = (string)Request.RequestContext.RouteData.Values["detailsSegments"];

            var slug = rawSlug.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            _legacyDetailsRedirectLogger?.Log(Request.RequestContext.HttpContext.Request.Url?.ToString());

            return new RedirectResult($"~/editorial/details/{slug}/", true);
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