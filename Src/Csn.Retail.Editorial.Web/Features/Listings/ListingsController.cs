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


        public ListingsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher, ISeoListingUrlRedirectLogger seoListingUrlRedirectLogger)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _seoListingUrlRedirectLogger = seoListingUrlRedirectLogger;
        }

        [Route("editorial/{articleSlug:article-types}")]
        [RedirectAttributeFilter]
        public async Task<ActionResult> ArticleTypeListing(ArticleTypeListingQuery query)
        {
            // temporary solution until we build proper landing pages
            var listingsQuery = _queryDispatcher.Dispatch<ArticleTypeListingQuery, GetListingsQuery>(query);

            return await Listing(listingsQuery);
        }

        [Route("editorial/{resultsPath:regex(^(results|beta-results))}/{*seoFragment:regex(^[\\w-/]*)?}")]
        [RedirectAttributeFilter]
        [LegacyListingsUrlRedirectFilter]
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
    }

    public class ListingsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}