using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Listings.Filters;
using Csn.Retail.Editorial.Web.Features.Listings.Loggers;
using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.Retail.Editorial.Web.Infrastructure.Wrappers;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class ListingsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;
        private readonly ISeoListingUrlRedirectLogger _seoListingUrlRedirectLogger;
        private readonly ILatamInherentListingRedirectLogger _latamInherentListingRedirectLogger;
        private readonly IArticleTypeLookup _articleTypeLookup;
        private readonly IUrlRouteHelper _urlRouteHelper;


        public ListingsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher,
            ISeoListingUrlRedirectLogger seoListingUrlRedirectLogger,
            ILatamInherentListingRedirectLogger latamInherentListingRedirectLogger,
            IArticleTypeLookup articleTypeLookup, IUrlRouteHelper urlRouteHelper)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
            _seoListingUrlRedirectLogger = seoListingUrlRedirectLogger;
            _latamInherentListingRedirectLogger = latamInherentListingRedirectLogger;
            _articleTypeLookup = articleTypeLookup;
            _urlRouteHelper = urlRouteHelper;
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
        /// An exceptional case that needs to be handled for just for redirects
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public ActionResult LatamInherentListing(GetListingsQuery query)
        {
            _latamInherentListingRedirectLogger.Log(HttpContext.Request.Url?.AbsolutePath);

            var path = HttpContext.Request.Url?.AbsolutePath;
            var urlFragments = path?.Trim('/').Split('/').Where(a => !string.IsNullOrEmpty(a)).ToList();

            if (urlFragments?.Count() > 2) // redirect /noticias/listado/actualidad/ to /noticias/actualidad/
            {
                var articleType = _articleTypeLookup.GetArticleTypeFromSlug(urlFragments[2]);
                if (articleType != null)
                    return new RedirectResult($"{path.Replace("/listado", string.Empty)}", true);
            }
            return new RedirectResult(_urlRouteHelper.HttpRouteUrl(RouteNames.Mvc.LandingHome, null), true);
        }
    }

    public class ListingsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}