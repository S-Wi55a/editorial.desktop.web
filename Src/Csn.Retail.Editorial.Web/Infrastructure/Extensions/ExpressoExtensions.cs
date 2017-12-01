using System;
using System.Linq;
using Expresso.Expressions;
using Expresso.Expressions.Visitors;
using Expresso.Sanitisation;
using Expresso.Syntax;
using Expresso.Syntax.Binary;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ExpressoExtensions
    {
        public static Expression AppendOrUpdateKeywords(this Expression source, string keyword)
        {
            var newExpression = !string.IsNullOrEmpty(keyword) ? new KeywordExpression("Keywords", keyword) : Expression.Create();
            var visitor = ReplacingVisitorBuilder
                .Find(e => e is KeywordExpression)
                .Substitution(e => newExpression)
                .WhenNotFound(e => e & newExpression)
                .Build();

                return visitor.ReplaceIn(source);
        }

        public static string GetKeywords(this Expression source)
        {
            var newExpression = source is BranchExpression expression ? expression.Expressions.FirstOrDefault(a => a is KeywordExpression) : null;
            return newExpression != null ? ((KeywordExpression) newExpression).Right : string.Empty;
        }

        public static bool IsRyvussBinaryTreeSyntax(this string query)
        {
            if (string.IsNullOrEmpty(query)) return false;

            var parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser());

            try
            {
                parser.Parse(query);
                return true;
            }
            catch
            {
                //An exception shows it isn't parsed by binary - therefore is V4
            }

            return false;
        }

        public static Expression TryParse(this IExpressionParser parser, string input)
        {
            try
            {
                return parser.Parse(input);
            }
            catch (Exception)
            {
                // ignored
            }

            return null;
        }
    }
}