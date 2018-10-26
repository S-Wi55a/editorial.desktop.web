using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ImageExtensions
    {
        public static string GetHeroImageUrl(this Image image, int width=0, int height=0)
        {
            var queryString = new Dictionary<string, string>();

            if (width > 0)
                queryString.Add("width", width.ToString());

            if (height > 0)
                queryString.Add("height", height.ToString());

            if (image.NeedsCropping)
                queryString.Add("pxc_method", "crop");

            var imageUrl = string.Format(image.Url + "?{0}", string.Join("&", queryString.Select(kvp => $"{kvp.Key}={kvp.Value}")));

            return imageUrl;
        }
    }
}