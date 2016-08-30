using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Infrastructure.HealthChecks
{
    public class HealthCheckSummary
    {
        public bool IsHealthy { get; set; }
        public IEnumerable<IHealthCheckResult> Results { get; set; }
    }
}