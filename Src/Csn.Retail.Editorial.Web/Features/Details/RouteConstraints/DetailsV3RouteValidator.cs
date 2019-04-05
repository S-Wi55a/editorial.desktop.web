using System;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.RouteConstraints
{
    public interface IDetailsV3RouteValidator
    {
        bool IsValid(string relativePath);
    }

    /// <summary>
    /// V3 routing looks like /noticias/detalle/this-is-a-test-article--1234/
    /// </summary>
    [AutoBind]
    public class DetailsV3RouteValidator : IDetailsV3RouteValidator
    {
        private readonly IEditorialRouteSettings _editorialSettings;

        public DetailsV3RouteValidator(IEditorialRouteSettings editorialSettings)
        {
            _editorialSettings = editorialSettings;
        }

        public bool IsValid(string relativePath)
        {
            var urlSegments = relativePath.Trim('/').Split('/');

            return urlSegments.Length == 2 && SatisfyRouteConstraints(urlSegments.First(), urlSegments.Last());
        }

        private bool SatisfyRouteConstraints(string prefix, string articleId)
        {
            if (string.Compare(prefix, _editorialSettings.DetailsRouteSegment, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return false;
            }

            var articleIdRegex = new Regex("^.*--\\d+/?$", RegexOptions.CultureInvariant);

            return articleIdRegex.IsMatch(articleId);
        }
    }
}