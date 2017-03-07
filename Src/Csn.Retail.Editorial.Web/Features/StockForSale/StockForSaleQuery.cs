using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.StockForSale
{
    public class StockForSaleQuery : IQuery
    {
        public string Uri { get; set; }
    }
}