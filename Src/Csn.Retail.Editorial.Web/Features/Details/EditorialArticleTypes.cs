using System;
using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.Details
{
    public enum EditorialArticleType
    {
        Miscellaneous,
        CarAdvice,
        Advice,
        RidingAdvice,
        Engine,
        Features,
        News,
        Product,
        Products,
        Reviews,
        Tips,
        TowTests,
        Video,
        Motoracing,
    }

    public class EditorialArticleTypes
    {
        public static IDictionary<EditorialArticleType, string> ConstraintsDictionary =>
            new Dictionary<EditorialArticleType, string>
            {
                {EditorialArticleType.Advice, "advice"},
                {EditorialArticleType.CarAdvice, "advice"},
                {EditorialArticleType.RidingAdvice, "riding-advice"},
                {EditorialArticleType.Engine, "engine-reviews"},
                {EditorialArticleType.Features, "features"},
                {EditorialArticleType.News, "news"},
                {EditorialArticleType.Product, "products"},
                {EditorialArticleType.Products, "products"},
                {EditorialArticleType.Reviews, "reviews"},
                {EditorialArticleType.Tips, "tips"},
                {EditorialArticleType.TowTests, "tow-tests"},
                {EditorialArticleType.Video, "videos"},
                {EditorialArticleType.Miscellaneous, "news"},
                {EditorialArticleType.Motoracing, "motoracing"},
            };

        public static EditorialArticleType? GetEditorialArticleType(string constraintValue)
        {
            if (ConstraintsDictionary.Any(v => v.Value.Equals(constraintValue, StringComparison.OrdinalIgnoreCase)))
            {
                return ConstraintsDictionary.FirstOrDefault(v => v.Value.Equals(constraintValue, StringComparison.OrdinalIgnoreCase)).Key;
            }
            return null;
        }
    }
}