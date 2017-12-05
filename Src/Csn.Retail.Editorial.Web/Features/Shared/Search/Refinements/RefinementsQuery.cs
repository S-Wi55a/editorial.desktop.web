using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    public class RefinementsQuery : IQuery
    {
        public string Q { get; set; }
        public string RefinementAspect { get; set; }
        public string ParentExpression { get; set; }
        public string Sort { get; set; }
    }
}