using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Listings.Filters;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class ListingsController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;


        public ListingsController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
        }

        [Route("editorial/beta-results/")]
        [RedirectAttributeFilter]
        [LegacyListingsUrlRedirectFilter]
        public async Task<ActionResult> Index(GetListingsQuery query)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new ListingsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetListingsQuery, GetListingsResponse>(query);

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response != null)
            {
                return View("ListingTemplate", response.ListingsViewModel);
            }

            var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

            return await errorsController.ErrorGeneric();
        }
    }

    public class ListingsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}