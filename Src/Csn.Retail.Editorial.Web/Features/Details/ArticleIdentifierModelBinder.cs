using System;
using System.Linq;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [ModelBinderType(typeof(ArticleIdentifierV1V3))]
    public class ArticleIdentifierV1V3ModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var detailsPath = bindingContext.ValueProvider.GetValue("detailsPath");

            if (detailsPath == null) return base.BindModel(controllerContext, bindingContext);

            if (!(base.BindModel(controllerContext, bindingContext) is ArticleIdentifierV1V3 record)) return null;

            var slug = detailsPath.RawValue?.ToString().Trim('/');
            var id = slug?.Split('-').Last() ?? string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                var settings = DependencyResolver.Current.GetService<IEditorialRouteSettings>();

                record.NetworkId = settings.NetworkIdFormat.FormatWith(id);
                record.Slug = slug?.Split('/').LastOrDefault();
            }

            UpdateEditorialIdInRoute(controllerContext, record.NetworkId);

            return record;
        }

        private void UpdateEditorialIdInRoute(ControllerContext controllerContext, string id)
        {
            UpdateRouteValue(controllerContext, "networkId", id);
        }

        private void UpdateRouteValue(ControllerContext controllerContext, string key, string value)
        {
            if (!controllerContext.RouteData.Values.ContainsKey(key))
            {
                controllerContext.RouteData.Values.Add(key, value);
            }
        }

        public bool CanBind(Type modelType)
        {
            return typeof(ArticleIdentifierV1V3).IsAssignableFrom(modelType);
        }
    }

    [ModelBinderType(typeof(ArticleIdentifierV2))]
    public class ArticleIdentifierV2ModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var detailsPath = bindingContext.ValueProvider.GetValue("detailsPath");

            if (detailsPath == null) return base.BindModel(controllerContext, bindingContext);

            if (!(base.BindModel(controllerContext, bindingContext) is ArticleIdentifierV2 record)) return null;

            var networkId = detailsPath.RawValue?.ToString().Trim('/').Split('/').LastOrDefault() ?? string.Empty;

            if (!string.IsNullOrEmpty(networkId))
            {
                record.NetworkId = networkId;
            }

            UpdateEditorialIdInRoute(controllerContext, record.NetworkId);

            return record;
        }

        private void UpdateEditorialIdInRoute(ControllerContext controllerContext, string id)
        {
            UpdateRouteValue(controllerContext, "networkId", id);
        }

        private void UpdateRouteValue(ControllerContext controllerContext, string key, string value)
        {
            if (!controllerContext.RouteData.Values.ContainsKey(key))
            {
                controllerContext.RouteData.Values.Add(key, value);
            }
        }

        public bool CanBind(Type modelType)
        {
            return typeof(ArticleIdentifierV2).IsAssignableFrom(modelType);
        }
    }
}