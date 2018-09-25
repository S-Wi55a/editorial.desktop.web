using System;
using System.Linq;
using System.Text.RegularExpressions;
using Bolt.Common.Extensions;
using Csn.Retail.Editorial.Web.Features.Shared.Settings;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;

namespace Csn.Retail.Editorial.Web.Features.Details.RouteConstraints
{
    public interface IDetailsV2RouteValidator
    {
        bool IsValid(string relativePath);
    }

    /// <summary>
    /// V2 routing looks like /editorial/details/this-is-a-test-article/ED-ITM-1234/
    /// </summary>
    [AutoBind]
    public class DetailsV2RouteValidator : IDetailsV2RouteValidator
    {
        private readonly IEditorialRouteSettings _editorialSettings;

        public DetailsV2RouteValidator(IEditorialRouteSettings editorialSettings)
        {
            _editorialSettings = editorialSettings;
        }

        public bool IsValid(string relativePath)
        {
            var urlSegments = relativePath.Trim('/').Split('/');

            return urlSegments.Length == 3 && SatisfyRouteConstraints(urlSegments.First(), urlSegments.Last());
        }

        private bool SatisfyRouteConstraints(string prefix, string articleId)
        {
            if (string.Compare(prefix, _editorialSettings.DetailsRouteSegment, StringComparison.InvariantCultureIgnoreCase) != 0)
            {
                return false;
            }

            var articleIdRegex = new Regex(_editorialSettings.NetworkIdFormat.FormatWith("\\d+/?$"), RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

            return articleIdRegex.IsMatch(articleId);
        }
    }
}