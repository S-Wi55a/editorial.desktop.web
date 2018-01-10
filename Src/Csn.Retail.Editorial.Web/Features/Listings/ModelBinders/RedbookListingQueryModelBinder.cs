using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            var routeValue = bindingContext.ValueProvider.GetValue("redbookvertical");

            if (!(base.BindModel(controllerContext, bindingContext) is GetListingsQuery record)) return null;

            if (routeValue == null) return null;

            if (Enum.TryParse<RedbookVertical>(routeValue.AttemptedValue, true, out var vertical))
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