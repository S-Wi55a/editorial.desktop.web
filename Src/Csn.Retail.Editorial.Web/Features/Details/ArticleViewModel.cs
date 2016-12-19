using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class ArticleViewModel
    {
        public ArticleTemplateType ArticleTemplateType { get; set; }
        public string Headline { get; set; }
        public string SubHeading { get; set; }
        public string Summary { get; set; }
        public HeroSection HeroSection { get; set; }
        public List<Contributor> Contributors { get; set; }
    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public string BrightcoveVideoIFrameUrl { get; set; }
    }

}