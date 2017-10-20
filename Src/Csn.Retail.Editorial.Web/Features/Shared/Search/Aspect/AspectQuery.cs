using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Aspect
{
    public class AspectQuery : IQuery
    {
        public string Query { get; set; }
        public string Aspect { get; set; }
        public string SortOrder { get; set; }
    }
}