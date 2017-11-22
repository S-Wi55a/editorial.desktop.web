using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Nav
{
    public class NavQuery : IQuery
    {
        public string Query { get; set; }
        public string Sort { get; set; }
    }
}