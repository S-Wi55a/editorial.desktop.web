using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Listings.Filters;
using Csn.Retail.Editorial.Web.Features.Listings.Loggers;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class ListingsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISeoListingUrlRedirectLogger _seoListingUrlRedirectLogger;

        public ListingsController(IQueryDispatcher queryDispatcher,
                IEventDispatcher eventDispatcher,
                ISeoListingUrlRedirectLogger seoListingUrlRedirectLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _seoListingUrlRedirectLogger = seoListingUrlRedirectLogger;
        }

        [RedirectAttributeFilter]
        [RedbookDefaultVerticalAttributeFilter]
        public async Task<ActionResult> ArticleTypeListing(ArticleTypeListingQuery query)
        {
            var listingsQuery = _queryDispatcher.Dispatch<ArticleTypeListingQuery, GetListingsQuery>(query);

            return await Listing(listingsQuery);
        }

        [RedirectAttributeFilter]
        public async Task<ActionResult> RedbookListing(RedbookListingQuery query)
        {
            var listingsQuery = _queryDispatcher.Dispatch<RedbookListingQuery, GetListingsQuery>(query);

            return await Listing(listingsQuery);
        }

        [RedirectAttributeFilter]
        [LegacyListingsUrlRedirectFilter]
        [SeoFriendlyRoutesRedirectFilter]
        public async Task<ActionResult> Listing(GetListingsQuery query)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new ListingsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetListingsQuery, GetListingsResponse>(query);

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response == null)
            {
                var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
                errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

                return await errorsController.ErrorGeneric();
            }

            if (response.RedirectRequired)
            {
                _seoListingUrlRedirectLogger.Log(HttpContext.Request.Url?.ToString());

                return new RedirectResult(response.RedirectUrl, true);
            }

            return View("ListingTemplate", response.ListingsViewModel);
        }

        /// <summary>
        /// Have a new controller and action which handles the generically configured redirects.
        /// We will have a dictionary of incoming routes and the route to redirect to.
        ///
        /// We just need to think about where in the order of routes this check will be added. If its too
        /// late then we may not be able to pick up all the redirects. If its too early then there is some performance
        /// overhead.
        ///
        /// Have a config for article types which includes the following:
        /// - Query for listing page
        /// - Slug (used to register the route)
        ///
        /// For the redirects use the generically configured redirects
        ///
        /// Article Type Labels:
        /// - Ad an additional property called Label which will come from Ryvuss and Data service. Then we don't need
        /// front end logic for this
        ///
        /// STEPS:
        /// 1. Implement generic redirects
        /// 2. Implement Article Label
        /// 3. Implement article type listing based on article type config (need article type specific config
        ///    because we need to be able to override the nav)
        /// </summary>
        /// <returns></returns>
        
    }

    public class ListingsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}