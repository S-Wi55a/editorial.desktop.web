using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.SiteNav
{
    public class SiteNavResponse
    {
        public SiteNavData Data { get; set; }
    }

    public class SiteNavData
    {
        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }
    }

}