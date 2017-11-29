using System.Web.Mvc;
using Csn.Retail.Editorial.Web.Features.Listings.ModelBinders;
using Csn.SimpleCqrs;
using Csn.WebMetrics.Core.Model;
using Expresso.Expressions;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    [ModelBinder(typeof(GetListingsQueryModelBinder))]
    public class GetListingsQuery : IQuery
    {
        public string Query { get; set; }
        public Expression QueryExpression { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Keywords { get; set; }

        public string SeoFragment { get; set; }

        public SearchEventType SearchEventType { get; set; }
    }
}