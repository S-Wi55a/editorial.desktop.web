using Csn.Tracking.Scripts.Core;
using Csn.WebMetrics.Core.Model;

namespace Csn.Retail.Editorial.Web.Features.Shared.Extensions
{
    public static class SearchActionExtensions
    {
        public static string ToTrackingScriptPageType(this SearchEventType searchEventType)
        {
            switch (searchEventType)
            {
                case SearchEventType.Search:
                    return TrackingScriptPageTypes.Search;
                case SearchEventType.Pagination:
                    return TrackingScriptPageTypes.Pagination;
                case SearchEventType.Refinement:
                    return TrackingScriptPageTypes.Refinement;
                case SearchEventType.Sort:
                    return TrackingScriptPageTypes.Sort;
            }

            return TrackingScriptPageTypes.Search;
        }
    }
}