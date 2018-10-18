using System;
using System.Globalization;
using Csn.Retail.Editorial.Web.Culture;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public static class DateAvailableMapper
    {
        public static string MapDateAvailable(this SearchResultDto source)
        {
            var culture = new CultureInfo(LanguageResourceValueProvider.GetUiCulture());
            if(source.DateAvailable.Year < DateTime.Now.Year)
            {
                return string.Format(culture, LanguageResourceValueProvider.GetValue(LanguageConstants.MonthYearFormat),
                    culture.TextInfo.ToTitleCase(source.DateAvailable.ToString("MMMM", culture)),
                    culture.TextInfo.ToTitleCase(source.DateAvailable.ToString("yyyy", culture)));
            }
            return GetFormatterDateWithOrdinalSuffix(culture, source, source.DateAvailable.Day);
        }

        private static string GetFormatterDateWithOrdinalSuffix(CultureInfo culture, SearchResultDto source,
            int dayOfTheMonth)
        {
            if (!culture.Name.StartsWith("en")) // 7 de Octubre
                return string.Format(culture, LanguageResourceValueProvider.GetValue(LanguageConstants.MonthDateFormat), source.DateAvailable.Day, culture.TextInfo.ToTitleCase(source.DateAvailable.ToString("MMMM", culture)));

            var postfix = dayOfTheMonth.ToString().EndsWith("11") || dayOfTheMonth.ToString().EndsWith("12") || dayOfTheMonth.ToString().EndsWith("13") ? "th" :
                dayOfTheMonth.ToString().EndsWith("1") ? "st" :
                dayOfTheMonth.ToString().EndsWith("2") ? "nd" :
                dayOfTheMonth.ToString().EndsWith("3") ? "rd" : "th";

            return string.Format(culture, LanguageResourceValueProvider.GetValue(LanguageConstants.MonthDateFormat), source.DateAvailable.ToString("MMMM", culture), source.DateAvailable.Day) + postfix;
        }
    }
}