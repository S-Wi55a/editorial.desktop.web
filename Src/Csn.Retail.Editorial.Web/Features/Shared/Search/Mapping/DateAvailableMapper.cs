using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public static class DateAvailableMapper 
    {
        public static string MapDateAvailable(this SearchResultDto source)
        {
            return source.DateAvailable.ToString("MMMM yyyy");
        }
    }
}