namespace Csn.Retail.Editorial.Web.Features.DisplayAds.Models
{
    public enum DisplayAdsSource
    {
        MediaMotive,
        GoogleAds
    }
    public interface IDisplayAdsModel
    {
        DisplayAdsSource DisplayAdsSource { get; set; }
    }
}