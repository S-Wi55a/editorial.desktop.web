using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class MoreArticleItem
    {
        public MoreArticleItemHeadings Headings { get; set; }
        public List<MoreArticleItems> MoreArticleItems { get; set; }
    }


    public class MoreArticleItemHeadings
    {
        public string ShowHeading { get; set; }
        public string HideHeading { get; set; }
    }


    public class MoreArticleItems
    {
        public string Title { get; set; }
        public string Uri { get; set; }
    }
}