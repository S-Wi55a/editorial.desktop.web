using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IDateAvailableMapper
    {
        string MapDateAvailable(SearchResultDto source);
    }

    [AutoBind]
    public class DateAvailableMapper : IDateAvailableMapper
    {
        private readonly EditorialSettings _editorialSettings;

        public DateAvailableMapper(EditorialSettings editorialSettings)
        {
            _editorialSettings = editorialSettings;
        }

        public string MapDateAvailable(SearchResultDto source)
        {
            return source.DateAvailable.ToString(_editorialSettings.TilePublishDateFormat);
        }
    }
}