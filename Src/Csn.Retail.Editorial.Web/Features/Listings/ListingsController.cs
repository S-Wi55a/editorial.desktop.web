using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialRyvussApi;
using Csn.Retail.Editorial.Web.Features.Shared.Search;
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

        [System.Web.Mvc.Route("editorial/listings/")]
        [RedirectAttributeFilter]
        // GET: Details
        public async Task<ActionResult> Index([FromUri]string q = null)
        {
            var dispatchedEvent = _eventDispatcher.DispatchAsync(new ListingsPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<SearchQuery, RyvussResult>(new SearchQuery()
            {
                Query = q
            });

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            //if (response != null)
            //{
                return View("ListingTemplate", response);
            //}

            //var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
            //errorsController.ControllerContext = new ControllerContext(this.Request.RequestContext, errorsController);

            //return await (response.HttpStatusCode == HttpStatusCode.NotFound ? errorsController.Error404() : errorsController.ErrorGeneric());
        }
    }

    public class ListingsPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}