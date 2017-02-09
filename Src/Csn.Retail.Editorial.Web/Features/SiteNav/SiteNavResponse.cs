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
        public static SiteNavData Empty = new SiteNavData
        {
            Footer = string.Empty,
            Script = string.Empty,
            Style = string.Empty,
            TopNav = string.Empty
        };

        public string Script { get; set; }
        public string Style { get; set; }
        public string TopNav { get; set; }
        public string Footer { get; set; }


    }
}