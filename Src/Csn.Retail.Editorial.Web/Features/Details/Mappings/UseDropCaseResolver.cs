using System.Linq;
using AutoMapper;
using Csn.Retail.Editorial.Web.Features.Details.Models;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using HtmlAgilityPack;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public class UseDropCaseResolver : IValueResolver<ArticleDetailsDto, ArticleViewModel, bool>
    {
        public bool Resolve(ArticleDetailsDto source, ArticleViewModel destination, bool destMember, ResolutionContext context)
        {
            // only use drop case if the first paragraph contains more than 255 chars
            if (!source.ContentSections.IsNullOrEmpty() && GetCharacterCountOfFirstParagraph(source.ContentSections.First()) >= 240)
            {
                return true;
            }

            return false;
        }

        private int GetCharacterCountOfFirstParagraph(ContentSection contentSection)
        {
            if (contentSection.SectionType != ContentSectionType.Html)
            {
                return 0;
            }
                
            // load the content as HTML and check the length of the first node
            var doc = new HtmlDocument();
            doc.LoadHtml(contentSection.Content);

            var result = doc.DocumentNode.FirstChild.GetFirstPararaphText();

            return string.IsNullOrEmpty(result) ? 0 : result.Length;
        }
    }
}