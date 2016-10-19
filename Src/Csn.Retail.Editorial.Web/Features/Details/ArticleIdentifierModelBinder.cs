using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class ArticleIdentifierModelBinder : DefaultModelBinder //, IFilteredModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var pageName = bindingContext.ValueProvider.GetValue("pageName");

            if (pageName == null) return base.BindModel(controllerContext, bindingContext);

            var record = base.BindModel(controllerContext, bindingContext) as ArticleIdentifier;

            if (record == null) return null;

            var id = pageName.RawValue?.ToString().Split('-').Last() ?? string.Empty;

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