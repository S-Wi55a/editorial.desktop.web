using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.Tracking.GoogleAnalytics
{
    public class GoogleAnalyticsDetailsModel
    {
        public string MemberTrackingId { get; set; }
        public string NetworkId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string PublishDate { get; set; }
        public string ContentGroup1 { get; set; }
        public string ContentGroup2 { get; set; }
        public string ContentGroup3 { get; set; }
        public string ScriptBlock { get; set; }
    }
}