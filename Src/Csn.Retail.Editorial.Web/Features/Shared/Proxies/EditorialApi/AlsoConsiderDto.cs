using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class AlsoConsiderDto
    {
        public string Heading { get; set; }
        public List<AlsoConsiderItems> AlsoConsiderItems { get; set; }
    }

    public class AlsoConsiderItems
    {
        public string LinkText { get; set; }
        public string Description { get; set; }
        public Image Image { get; set; }
        public string ArticleUrl { get; set; }
    }

}