using System;
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
            // only use drop case if the first paragraph contains more than 240 chars
            if (!source.ContentSections.IsNullOrEmpty() && CanApplyDropCase(source.ContentSections.First()))
            {
                return true;
            }

            return false;
        }

        private bool CanApplyDropCase(ContentSection contentSection)
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