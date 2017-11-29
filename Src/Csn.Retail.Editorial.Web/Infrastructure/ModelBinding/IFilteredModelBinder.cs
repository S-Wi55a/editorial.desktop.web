using System;
using System.Web.Mvc;

namespace Csn.Retail.Editorial.Web.Infrastructure.ModelBinding
{
    public interface IFilteredModelBinder : IModelBinder
    {
        bool CanBind(Type modelType);
    }
}