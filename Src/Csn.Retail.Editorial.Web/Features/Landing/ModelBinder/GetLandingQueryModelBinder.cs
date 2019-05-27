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
    public class GetLandingQueryModelBinder : DefaultModelBinder
    {
        private readonly ILandingConfigProvider _landingConfigProvider;

        public GetLandingQueryModelBinder(ILandingConfigProvider landingConfigProvider)
        {
            _landingConfigProvider = landingConfigProvider;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var slugRouteKey = bindingContext.ValueProvider.GetValue("slug");

            var landingConfig = _landingConfigProvider.GetConfig(slugRouteKey == null ? "" : slugRouteKey.AttemptedValue.Trim('/'));

            if (landingConfig.Result == null) return new GetLandingQuery
            {
                PromotionId = GetPromotionId(bindingContext)
            };

            if (landingConfig.Result.CarouselConfigurations == null || !landingConfig.Result.CarouselConfigurations.Any())
            {
                controllerContext.HttpContext.Response.Redirect(ListingUrlHelper.GetSeoUrl("/" + landingConfig.Result.Slug));
            }

            return new GetLandingQuery
            {
                Configuration = landingConfig.Result,
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