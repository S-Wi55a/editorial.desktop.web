using Csn.Retail.Editorial.Web.Features.Shared.Helpers;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IImageMapper
    {
        string MapImageUrl(SearchResultDto source);
    }
    public class ImageMapper : IImageMapper
    {
        private readonly IImageUrlHelper _imageUrlHelper;

        public ImageMapper(IImageUrlHelper imageUrlHelper)
        {
            _imageUrlHelper = imageUrlHelper;
        }

        public string MapImageUrl(SearchResultDto source)
        {
            return _imageUrlHelper.GetImageUrl(source.PhotoPath);
        }
    }
}