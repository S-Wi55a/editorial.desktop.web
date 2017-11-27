using Csn.Retail.Editorial.Web.Features.Shared.Search.Shared;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IResultsMessageMapper
    {
        string MapResultMessage(RyvussNavResultDto source);
        string MapNoResultMessage(int count);
        string MapNoResultInstructionMessage(int count);
    }

    [AutoBind]
    public class ResultsMessageMapper: IResultsMessageMapper
    {
        public string MapResultMessage(RyvussNavResultDto source)
        {
            if (!string.IsNullOrEmpty(source.Metadata?.H1))
            {
                return source.Metadata.H1;
            }

            if (source.Count < 1) return null;

            return source.Count > 1 ? $"{source.Count} Articles found" : "1 Article found";
        }

        public string MapNoResultInstructionMessage(int count)
        {
            return count < 1 ? "Please refine your search by removing a breadcrumb." : string.Empty;
        }

        public string MapNoResultMessage(int count)
        {
            return count < 1 ? "0 Articles found. Unfortunately we can't find any articles that suit your refinement." : string.Empty;
        }
    }
}