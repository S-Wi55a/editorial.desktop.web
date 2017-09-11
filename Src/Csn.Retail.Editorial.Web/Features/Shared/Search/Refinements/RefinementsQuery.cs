using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    public class RefinementsQuery : IQuery
    {
        public string Query { get; set; }
        public string Aspect { get; set; }
        public string AspectRefinement { get; set; }
        public string ParentExpression { get; set; }
    }
}