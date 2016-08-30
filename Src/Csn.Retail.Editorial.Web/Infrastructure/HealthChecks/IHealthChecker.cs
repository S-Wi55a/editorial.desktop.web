using System.Threading.Tasks;

namespace Csn.Retail.Editorial.Web.Infrastructure.HealthChecks
{
    public interface IHealthChecker
    {
        Task<IHealthCheckResult> CheckAsync();
    }


}