using System.Linq;
using Csn.Retail.Editorial.Web.Infrastructure.Attributes;
using Csn.Retail.Editorial.Web.Infrastructure.Extensions;
using Expresso.Expressions;
using Expresso.Syntax;

namespace Csn.Retail.Editorial.Web.Features.Shared.Helpers
{
    public interface IKeywordExpressionHelper
    {
        string AppendOrUpdate(string query, string keyword);
    }

    [AutoBind]
    public class KeywordExpressionHelper : IKeywordExpressionHelper
    {
        private readonly IExpressionParser _parser;
        private readonly IExpressionFormatter _expressionFormatter;

        public KeywordExpressionHelper(IExpressionParser parser, IExpressionFormatter expressionFormatter)
        {
            _parser = parser;
            _expressionFormatter = expressionFormatter;
        }
        public string AppendOrUpdate(string query, string keyword)
        {
            var currentExpression = _parser.Parse(query);
            var currentKeywordExpression = Expression.Create();

            if (currentExpression != EmptyExpression.Instance && currentExpression is BranchExpression)
            {
                currentKeywordExpression = ((BranchExpression)currentExpression).Expressions.FirstOrDefault(a => a is KeywordExpression);
            }

            if (!keyword.IsNullOrEmpty())
            {
                var keywordexpression = new KeywordExpression("Keywords", $"({keyword})");
                if (currentKeywordExpression != null && currentKeywordExpression != EmptyExpression.Instance)
                {
                    ((BranchExpression)currentExpression).Expressions.Remove(currentKeywordExpression);
                }

                return _expressionFormatter.Format(currentExpression & keywordexpression);
            }

            if (currentKeywordExpression != null && currentKeywordExpression != EmptyExpression.Instance)
            {
                ((BranchExpression)currentExpression).Expressions.Remove(currentKeywordExpression);
                return _expressionFormatter.Format(currentExpression);
            }

            return query;
        }
    }
}