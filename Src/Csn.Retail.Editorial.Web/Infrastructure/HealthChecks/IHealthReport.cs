using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Infrastructure.HealthChecks
{
    public interface IHealthReporter
    {
        Task<HealthCheckSummary> ReportAsync();
    }

    [AutoBind]
    public class HealthReporter : IHealthReporter
    {
        private readonly IEnumerable<IHealthChecker> _healthCheckers;

        public HealthReporter(IEnumerable<IHealthChecker> healthCheckers)
        {
            _healthCheckers = healthCheckers ?? Enumerable.Empty<IHealthChecker>();
        }

        public async Task<HealthCheckSummary> ReportAsync()
        {
            var healthCheckTasks = _healthCheckers.Select(x => x.CheckAsync());

            var results = await Task.WhenAll(healthCheckTasks);

            var fatalSeverityChecks = results.Where(x => x.FailureSeverity == FailureSeverity.Fatal).ToList();
            return new HealthCheckSummary
            {
                IsHealthy = fatalSeverityChecks.Count == 0 || fatalSeverityChecks.All(x => x.IsHealthy),
                Results = results
            };
        }
    }
}