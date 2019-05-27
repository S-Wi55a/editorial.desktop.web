using System.Collections.Generic;
using Csn.Retail.Editorial.Web.Features.Landing.Configurations;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Tracking.Scripts.Core;

namespace Csn.Retail.Editorial.Web.Features.Landing.Mappings
{
    public static class LandingInsightsDataMapper
    {
        public static CsnInsightsData Map(LandingConfigurationSet landingConfiguration)
        {
            return new CsnInsightsData
            {
                MetaData = new Dictionary<string, string>
                {
                    {TrackingScriptTags.ContentGroup1, TrackingScriptContentGroups.NewsAndReviews},
                    {TrackingScriptTags.ContentGroup2, landingConfiguration.ContentGroup}
                }
            };
        }
    }
}