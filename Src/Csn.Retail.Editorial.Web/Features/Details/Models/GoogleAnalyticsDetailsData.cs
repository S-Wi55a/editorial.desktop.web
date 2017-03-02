using System;

namespace Csn.Retail.Editorial.Web.Features.Details.Models
{
    public class GoogleAnalyticsDetailsData
    {
        public Guid? MemberTrackingId { get; set; }
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