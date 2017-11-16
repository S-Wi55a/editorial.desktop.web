using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsQuery : IQuery
    {
        public string Q { get; set; }
        public int Offset { get; set; }
        public string SortOrder { get; set; }
        public string Keyword { get; set; }
    }
}