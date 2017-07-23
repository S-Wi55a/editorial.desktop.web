using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search
{
    public class SearchQuery : IQuery
    {
        public string Query { get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
    }
}