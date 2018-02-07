namespace Csn.Retail.Editorial.Web.Features.Shared.HeroAdUnit.Models
{
    public class CampaignAdResult
    {
        public CampaignAdResult()
        {
            Data = new Data();
        }
   
        public Data Data { get; set; }

        public CampaingImpressionTracking Tracking { get; set; }
    }

    public class Data
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Title { get; set; }
        public string ExternalLinkText { get; set; }
        public string ExternalLinkUrl { get; set; }
        public string DesktopImageUrl { get; set; }
        public string Opacity { get; set; }
        
    }

    public class CampaingImpressionTracking
    {
        public string[] ViewDesktop  { get; set; }
    }
}