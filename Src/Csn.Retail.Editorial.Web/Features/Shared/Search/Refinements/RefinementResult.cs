using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    public class RefinementResult
    {
        public int Count { get; set; }
        public RefinementNav Nav { get; set; }
    }

    public class RefinementNav
    {
        public List<RefinementNavNode> Nodes { get; set; }
        public string KeywordsPlaceholder { get; set; }
    }

    public class RefinementNavNode : NavNode
    {
        public RefinementNavNode2 Refinements { get; set; }
    }

    public class RefinementNavNode2 : NavNode
    {
        public Refinement Refinement { get; set; }
    }
}