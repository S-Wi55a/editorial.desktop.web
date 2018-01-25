using System.Threading.Tasks;
using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Errors;
using Csn.Retail.Editorial.Web.Features.Shared.GlobalSite;
using Csn.Retail.Editorial.Web.Infrastructure.Filters;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    public class LandingController : Controller
    {
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly IEventDispatcher _eventDispatcher;


        public LandingController(IQueryDispatcher queryDispatcher, IEventDispatcher eventDispatcher)
        {
            _queryDispatcher = queryDispatcher;
            _eventDispatcher = eventDispatcher;
        }

        [Route("editorial/")]
        [RedirectAttributeFilter]
        public async Task<ActionResult> Index(GetLandingQuery query)
        {

            var dispatchedEvent = _eventDispatcher.DispatchAsync(new LandingPageRequestEvent());

            var dispatchedQuery = _queryDispatcher.DispatchAsync<GetLandingQuery, GetLandingResponse>(query);

            await Task.WhenAll(dispatchedEvent, dispatchedQuery);

            var response = dispatchedQuery.Result;

            if (response == null)
            {
                var errorsController = DependencyResolver.Current.GetService<ErrorsController>();
                errorsController.ControllerContext = new ControllerContext(Request.RequestContext, errorsController);

                return await errorsController.ErrorGeneric();
            }

            return View("LandingTemplate", response.LandingViewModel);
        }
    }

    public class LandingPageRequestEvent : IEvent, IRequireGlobalSiteNav, IRequiredGoogleAnalyticsTrackingScript
    {
    }
}