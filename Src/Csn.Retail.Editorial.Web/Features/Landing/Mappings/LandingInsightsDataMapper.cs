using System.Collections.Generic;
using Csn.Tracking.Scripts.Core;

namespace Csn.Retail.Editorial.Web.Features.Landing.Mappings
{
    public static class LandingInsightsDataMapper
    {
        public static Dictionary<string, string> Map()
        {
            return new Dictionary<string, string>
            {
                {TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews},
            };
        }
    }
}