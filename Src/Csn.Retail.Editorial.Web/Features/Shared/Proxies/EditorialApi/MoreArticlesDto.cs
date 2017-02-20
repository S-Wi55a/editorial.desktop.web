using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi
{
    public class MoreArticlesDto
    {
        public string Name { get; set; }
        public string ReadMore { get; set; }
        public List<LatestArticleItem> Items { get; set; }
        public string NextQuery { get; set; }
    }


    public class LatestArticleItem
    {
        public string Id { get; set; }
        public string Url { get; set; }
        public string Headline { get; set; }
        public LatestImage Image { get; set; }
    }

    public class LatestImage
    {
        public string Url { get; set; }
        public string AlternateText { get; set; }
    }
}