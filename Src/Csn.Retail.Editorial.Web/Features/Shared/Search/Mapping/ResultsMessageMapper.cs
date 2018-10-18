using Csn.MultiTenant;
using Csn.Retail.Editorial.Web.Culture;
using Csn.Retail.Editorial.Web.Features.Shared.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IResultsMessageMapper
    {
        string MapResultMessage(RyvussNavResultDto source);
        string MapNoResultInstructionMessage(int count);
    }

    [AutoBind]
    public class ResultsMessageMapper: IResultsMessageMapper
    {
        private readonly ITenantProvider<TenantInfo> _tenantProvider;

        public ResultsMessageMapper(ITenantProvider<TenantInfo> tenantProvider)
        {
            _tenantProvider = tenantProvider;
        }
        public string MapResultMessage(RyvussNavResultDto source)
        {
            if (!string.IsNullOrEmpty(source.Metadata?.H1))
            {
                return source.Metadata.H1;
            }

            return string.Format(LanguageResourceValueProvider.GetValue(LanguageConstants.NumberOfArticlesFoundFormat), _tenantProvider.Current().Culture, source.Count);
        }

        public string MapNoResultInstructionMessage(int count)
        {
            return count < 1 ? LanguageResourceValueProvider.GetValue(LanguageConstants.RefineSearchText) : string.Empty;
        }
    }
}