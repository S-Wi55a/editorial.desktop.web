namespace Csn.Retail.Editorial.Web.Features.DisplayAds
{
    public enum DisplayAdsSource
    {
        MediaMotive,
        GoogleAd
    }
    public interface IDisplayAdsModel
    {
        DisplayAdsSource DisplayAdsSource { get; set; }
    }
}