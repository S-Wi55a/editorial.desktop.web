namespace Csn.Retail.Editorial.Web.Infrastructure.Redirects
{
    public interface IMobiDetailsUrlBuilder
    {
        string Build(string editorialId);
        bool IsSupported(string tenant);
    }
}