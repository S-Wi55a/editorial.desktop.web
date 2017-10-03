using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Shared.Search.Mapping
{
    public interface IResultsMessageMapper
    {
        string MapResultMessage(int count);
        string MapResultInstructionMessage(int count);
    }

    [AutoBind]
    public class ResultsMessageMapper: IResultsMessageMapper
    {
        public string MapResultMessage(int count)
        {
            return count < 1 ? "0 Articles found. Unfortunately we can't find any articles that suit your refinement." : string.Empty;
        }

        public string MapResultInstructionMessage(int count)
        {
            return count < 1 ? "Please refine your search by removing a breadcrumb." : string.Empty;
        }
    }
}