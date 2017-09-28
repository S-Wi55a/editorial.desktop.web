using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    public class RefinementResult : NavNode
    {
        public int Count { get; set; }
        public RefinementNavNode Refinements { get; set; }
    }

    public class RefinementNavNode : NavNode
    {
        public Refinement Refinement { get; set; }
    }
}