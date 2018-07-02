using System;
using System.Linq;
using System.Web.Mvc;
using Autofac.Integration.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    [ModelBinderType(typeof(ArticleIdentifier))]
    public class ArticleIdentifierModelBinder : DefaultModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var articleSlug = bindingContext.ValueProvider.GetValue("slug");


            if (articleSlug == null) return base.BindModel(controllerContext, bindingContext);

            if (!(base.BindModel(controllerContext, bindingContext) is ArticleIdentifier record)) return null;

            var id = articleSlug.RawValue?.ToString().Trim('/').Split('-').Last() ?? string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                record.Id = $"ED-ITM-{id}";
            }

            UpdateEditorialIdInRoute(controllerContext, record.Id);

            return record;
        }

        private void UpdateEditorialIdInRoute(ControllerContext controllerContext, string id)
        {
            UpdateRouteValue(controllerContext, "id", id);
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
            return typeof(ArticleIdentifier).IsAssignableFrom(modelType);
        }
    }
}