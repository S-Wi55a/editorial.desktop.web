﻿using System;
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
        private readonly IDetailsRequestLogger _detailsRequestLogger;

        public DetailsController(IQueryDispatcher queryDispatcher, 
                                IEventDispatcher eventDispatcher, 
                                IDetailsRedirectLogger redirectLogger,
                                IDetailsRequestLogger detailsRequestLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _redirectLogger = redirectLogger;
            _detailsRequestLogger = detailsRequestLogger;
        }

        [RedirectAttributeFilter]
        // ReSharper disable once InconsistentNaming
        // Details V1 and V3 can use the same articleIdentifier model and model binder
        public Task<ActionResult> IndexDetailsV1V3(ArticleIdentifierV1V3 articleIdentifier, bool __preview = false)
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
            _detailsRequestLogger.Log(HttpContext.Request.Url?.ToString());

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

        public async Task<ActionResult> Modal(string networkId, string __source = "")
        {
            _detailsRequestLogger.Log(HttpContext.Request.Url?.ToString());

            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsModalRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetModalQuery, GetArticleResponse>(new GetModalQuery
            {
                Id = networkId.ToUpper(),
                Source = __source
            });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response.ArticleViewModel != null)
            {
                return View("DetailsModal/DetailsModalTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404Child() : errorsController.ErrorGenericChild();
        }

        public async Task<ActionResult> Preview(string previewId)
        {
            _detailsRequestLogger.Log(HttpContext.Request.Url?.ToString());

            var dispatchedEvent = _eventDispatcher.DispatchAsync(new DetailsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<PreviewQuery, GetArticleResponse>(new PreviewQuery()
            {
                Id = previewId
            });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response.ArticleViewModel != null)
            {
                return View("Preview/PreviewTemplate", response.ArticleViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404Child() : errorsController.ErrorGenericChild();
        }
    }

    public class DetailsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }

    public class DetailsModalRequestEvent : IEvent, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}