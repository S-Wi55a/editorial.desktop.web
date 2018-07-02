using System;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Listings.ModelBinders
{
    [ModelBinderType(typeof(RedbookListingQuery))]
    public class RedbookListingQueryModelBinder : GetListingsQueryModelBinder
    {
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var routeValue = bindingContext.ValueProvider.GetValue("redbookVertical");

            if (!(base.BindModel(controllerContext, bindingContext) is GetListingsQuery record)) return null;

            if (routeValue == null) return null;

            if (Enum.TryParse<Vertical>(routeValue.AttemptedValue.Trim('/'), true, out var vertical))
            {
                return new RedbookListingQuery
                {
                    Vertical = vertical,
                    Query = record.Query,
                    Keywords = record.Keywords,
                    Sort = record.Sort,
                    Offset = record.Offset,
                    EditorialPageType = record.EditorialPageType,
                    QueryExpression = record.QueryExpression,
                    SearchEventType = record.SearchEventType,
                    SeoFragment = record.SeoFragment

                };
            }
            return null;
        }

        public RedbookListingQueryModelBinder(IExpressionParser parser) : base(parser)
        {
        }
    }
}