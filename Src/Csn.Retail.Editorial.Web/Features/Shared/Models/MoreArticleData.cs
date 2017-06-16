using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class MoreArticleData
    {
        public string ShowText { get; set; }
        public string HideText { get; set; }

        public List<MoreArticleLink> Links { get; set; }

        public List<MoreArticleFilter> Filters { get; set; }
    }

    public class MoreArticleFilter
    {
        public string Title { get; set; }
        public string Uri { get; set; }
    }

    public class MoreArticleLink
    {
        public string Text { get; set; }
        public string Uri { get; set; }
    }
}