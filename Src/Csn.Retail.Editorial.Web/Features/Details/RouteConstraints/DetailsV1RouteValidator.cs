using System;
using System.Linq;
using System.Text.RegularExpressions;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.RouteConstraints
{
    public interface IDetailsV1RouteValidator
    {
        bool IsValid(string relativePath);
    }

    /// <summary>
    /// V2 routing looks like /editorial/details/this-is-a-test-article/ED-ITM-1234/
    /// </summary>
    [AutoBind]
    public class DetailsV1RouteValidator : IDetailsV1RouteValidator
    {
        private readonly IEditorialSettings _editorialSettings;

        public DetailsV1RouteValidator(IEditorialSettings editorialSettings)
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

            var articleIdRegex = new Regex("^.*-\\d+/?$", RegexOptions.CultureInvariant);

            return articleIdRegex.IsMatch(articleId);
        }
    }
}