using System.Collections.Generic;
using System.Linq;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IContributorMapper
    {
        List<Contributor> Map(ArticleDetailsDto article);
    }


    [AutoBind]
    public class ContributorMapper : IContributorMapper
    {
        public List<Contributor> Map(ArticleDetailsDto article)
        {
            return article.Contributors?.Select(contributor => new Contributor
            {
                Name = contributor.Name, Description = contributor.Description, ImageUrl = contributor.ImageUrl, LinkUrl = contributor.LinkUrl
            }).ToList();
        }
    }
}