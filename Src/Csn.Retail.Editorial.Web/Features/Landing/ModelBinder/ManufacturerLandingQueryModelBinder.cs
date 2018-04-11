using System;
using System.Linq;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations.Providers;
using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
namespace Csn.Retail.Editorial.Web.Features.Landing.ModelBinder
{
    [ModelBinderType(typeof(GetLandingQuery))]
    public class ManufacturerLandingQueryModelBinder : DefaultModelBinder
    {
        private readonly ILandingConfigProvider _landingConfigProvider;

        public ManufacturerLandingQueryModelBinder(ILandingConfigProvider landingConfigProvider)
        {
            _landingConfigProvider = landingConfigProvider;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var manufacturerRouteKey = bindingContext.ValueProvider.GetValue("manufacturer");

            if (manufacturerRouteKey == null) return new GetLandingQuery
            {
                PromotionId = GetPromotionId(bindingContext)
            };

            var manufacturer = _landingConfigProvider.LoadConfig(manufacturerRouteKey.AttemptedValue);

            if (manufacturer.Result == null) return new GetLandingQuery
            {
                PromotionId = GetPromotionId(bindingContext)
            };

            if (manufacturer.Result.CarouselConfigurations == null || !manufacturer.Result.CarouselConfigurations.Any())
            {
                controllerContext.HttpContext.Response.Redirect(ListingUrlHelper.GetSeoUrl("/" + manufacturer.Result.Type));
            }

            return new GetLandingQuery
            {
                Configuration = manufacturer.Result,
                PromotionId = GetPromotionId(bindingContext)
            };
        }

        private Guid? GetPromotionId(ModelBindingContext bindingContext)
        {
            var promotionId = bindingContext.ValueProvider.TryGetValueOrDefault("promotionId", string.Empty);
            return promotionId.IsNullOrEmpty() ? (Guid?) null : Guid.Parse(promotionId);
        }
    }
}