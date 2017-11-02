using System;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public static class DateAvailableMapper
    {

        private static int Year = DateTime.Now.Year;
        public static string MapDateAvailable(this SearchResultDto source)
        {
            if (source.DateAvailable.Year < Year)
            {
                return source.DateAvailable.ToString("MMMM yyyy");
            }
            return $"{source.DateAvailable:MMMM d}{source.DateAvailable.Day.GetOrdinalSuffix()}";
        }
        private static string GetOrdinalSuffix(this int num)
        {
            if (num.ToString().EndsWith("11")) return "th";
            if (num.ToString().EndsWith("12")) return "th";
            if (num.ToString().EndsWith("13")) return "th";
            if (num.ToString().EndsWith("1")) return "st";
            if (num.ToString().EndsWith("2")) return "nd";
            if (num.ToString().EndsWith("3")) return "rd";
            return "th";
        }
    }
}