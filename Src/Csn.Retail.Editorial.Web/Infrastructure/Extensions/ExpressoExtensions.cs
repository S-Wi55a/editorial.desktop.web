using Expresso.Expressions;
using Expresso.Expressions.Visitors;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class ExpressoExtensions
    {
        public static Expression AppendOrUpdateKeyword(this Expression source, string keyword)
        {
            var newExpression = string.IsNullOrEmpty(keyword) ? Expression.Create() : new KeywordExpression("Keywords", keyword);

            var visitor = ReplacingVisitorBuilder
                .Find(e => e is KeywordExpression)
                .Substitution(e => newExpression)
                .WhenNotFound(e => e & newExpression)
                .Build();

            return visitor.ReplaceIn(source);
        }
    }
}