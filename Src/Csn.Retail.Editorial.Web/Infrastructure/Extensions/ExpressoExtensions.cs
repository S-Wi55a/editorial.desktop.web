﻿using Expresso.Expressions;
using Expresso.Expressions.Visitors;
using Expresso.Sanitisation;
using Expresso.Syntax.Binary;

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

        public static bool IsRyvussBinaryTreeSyntax(this string query)
        {
            var parser = new FlatBinaryTreeParser(new BinaryTreeSanitiser());

            try
            {
                var parsed = parser.Parse(query);
                return true;
            }
            catch
            {
                //An exception shows it isn't parsed by binary - therefore is V4
            }

            return false;
        }
    }
}