using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search
{
    public class SearchQuery : IQuery
    {
        public string Query { get; set; }
    }
}