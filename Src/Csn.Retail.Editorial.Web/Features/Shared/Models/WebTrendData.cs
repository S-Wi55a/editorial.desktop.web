using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class WebTrendData
    {
        public string WebTrendId { get; set; }
        public string DomainName { get; set; }
        public List<WebTrendItem> WebTrendItems { get; set; }

        public class WebTrendItem
        {
            public string Name { get; set; }
            public string Content { get; set; }
        }
    }

}