using System;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing
{
    public class GetLandingQuery: IQuery
    {
        public Guid? PromotionId { get; set; }
        public LandingConfigurationSet Configuration { get; set; }
    }
}