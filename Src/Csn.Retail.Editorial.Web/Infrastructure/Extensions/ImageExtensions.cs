using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ImageExtensions
    {
        public static string GetHeroImageUrl(this Image image, int width=0, int height=0)
        {
            var imageUrl = $"{image.Url}?width={width}" + (height > 0 ? "&height=" + height : "") + (image.NeedsCropping ? "&pxc_method=crop" : "");

            return imageUrl;
        }
    }
}