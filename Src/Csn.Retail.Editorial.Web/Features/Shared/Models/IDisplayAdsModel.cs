namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public enum DisplayAdsTypes
    {
        MediaMotive,
        GoogleAds
    }
    public interface IDisplayAdsModel
    {
        DisplayAdsTypes DisplayAdsType { get; set; }
    }
}