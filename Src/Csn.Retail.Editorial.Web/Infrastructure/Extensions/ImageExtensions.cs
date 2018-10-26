using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ImageExtensions
    {
        public static string GetHeroImageUrl(this Image image, string width, string height)
        {
            var imageUrl = $"{image.Url}?width={width}&height={height}" + (image.NeedsCropping ? "&pxc_method=crop" : "");

            return imageUrl;
        }
    }
}