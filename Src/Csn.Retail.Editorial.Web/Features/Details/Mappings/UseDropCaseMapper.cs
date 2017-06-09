using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Csn.Retail.Editorial.Web.Features.Shared.Proxies.EditorialApi;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using HtmlAgilityPack;

namespace Csn.Retail.Editorial.Web.Features.Details.Mappings
{
    public interface IUseDropCaseMapper
    {
        bool Map(ArticleDetailsDto source);
    }

    [AutoBind]
    public class UseDropCaseMapper : IUseDropCaseMapper
    {
        public bool Map(ArticleDetailsDto source)
        {
            // only use drop case if the first paragraph contains more than 240 chars, and make sure that the first object
            // has a content - to avoid a null exception throw-out
            return !source.ContentSections.IsNullOrEmpty() && !source.ContentSections.First().Content.IsNullOrEmpty() &&
                   CanApplyDropCase(source.ContentSections.First());
        }

        private static bool CanApplyDropCase(ContentSection contentSection)
        {
            if (contentSection.SectionType != ContentSectionType.Html)
            {
                return false;
            }

            // load the content as HTML and check the length of the first node
            var doc = new HtmlDocument();
            doc.LoadHtml(contentSection.Content);

            var result = doc.DocumentNode.FirstChild.GetFirstPararaphText();

            if (string.IsNullOrEmpty(result))
            {
                return false;
            }

            if (!result.Substring(0, 1).All(char.IsLetter))
            {
                return false;
            }

            return result.Length > 240;
        }
    }
}