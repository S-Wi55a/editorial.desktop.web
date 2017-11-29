using System.Collections.Generic;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.ModelBinding
{
    public class SmartModelBinder : DefaultModelBinder
    {
        private readonly IEnumerable<IFilteredModelBinder> _filteredModelBinders;

        public SmartModelBinder(IEnumerable<IFilteredModelBinder> filteredModelBinders)
        {
            _filteredModelBinders = filteredModelBinders;
        }

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            foreach (var modelBinder in _filteredModelBinders)
            {
                if (modelBinder.CanBind(bindingContext.ModelType))
                {
                    return modelBinder.BindModel(controllerContext, bindingContext);
                }
            }

            return base.BindModel(controllerContext, bindingContext);
        }
    }
}