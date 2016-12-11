using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public class ArticleViewModel
    {
        public ArticleTemplateType ArticleTemplateType { get; set; }
        public string Headline { get; set; }
        public HeroSection HeroSection { get; set; }
    }
}