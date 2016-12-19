using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Details.Models
{
    public class ArticleViewModel
    {
        public ArticleTemplateType ArticleTemplateType { get; set; }
        public string Headline { get; set; }
        public HeroSection HeroSection { get; set; }
        public List<ContentSection> ContentSections { get; set; }
    }

    public class HeroSection
    {
        public HeroType Type { get; set; }
        public List<Image> Images { get; set; }
        public string BrightcoveVideoIFrameUrl { get; set; }
    }
}