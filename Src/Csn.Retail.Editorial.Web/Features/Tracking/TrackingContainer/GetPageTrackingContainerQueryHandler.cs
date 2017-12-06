using System.Web;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;
using Csn.WebMetrics.Editorial.Interfaces;

namespace Csn.Retail.Editorial.Web.Features.Tracking.TrackingContainer
{
    [AutoBind]
    public class GetPageTrackingContainerQueryHandler :
        IQueryHandler<GetPageTrackingContainerQuery, IAnalyticsTrackingContainer>
    {
        private readonly IEditorialLandingTrackingContainerProvider _provider;
        private readonly HttpContextBase _httpContext;

        public GetPageTrackingContainerQueryHandler(IEditorialLandingTrackingContainerProvider provider, HttpContextBase httpContext)
        {
            _provider = provider;
            _httpContext = httpContext;
        }

        public IAnalyticsTrackingContainer Handle(GetPageTrackingContainerQuery query)
        {
            return _provider.GetContainer(_httpContext);
        }
    }
}