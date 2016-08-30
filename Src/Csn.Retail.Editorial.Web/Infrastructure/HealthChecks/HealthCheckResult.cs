namespace Csn.Retail.Editorial.Web.Infrastructure.HealthChecks
{
    public class HealthCheckResult : IHealthCheckResult
    {
        public string Name { get; set; }
        public FailureSeverity FailureSeverity { get; set; }
        public bool IsHealthy { get; set; }
        public object Details { get; set; }
    }

    public interface IHealthCheckResult
    {
        string Name { get; }
        FailureSeverity FailureSeverity { get; }
        bool IsHealthy { get; }
    }

    public enum FailureSeverity
    {
        Fatal,
        Warn,
        Low
    }
}