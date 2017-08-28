using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Nav;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Refinements
{
    public class RefinementResult : NavNode
    {
        public int Count { get; set; }
        public NavNode Refinements { get; set; }
    }
}