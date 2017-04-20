using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public class PolarNativeAds
    {
        public string SectionName { get; set; }
        public string SitePropertyId { get; set; }
        public string SiteName { get; set; }
        public string AreaName { get; set; }
        public string ArticleType { get; set; }
    }


    public class PolarNativeAdsCutomValue
    {
        public string AreaName { get; set; }
        public string MakeModel { get; set; }
    }
}