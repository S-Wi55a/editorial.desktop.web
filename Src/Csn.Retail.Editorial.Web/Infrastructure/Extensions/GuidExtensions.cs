using System;

namespace Csn.Retail.Editorial.Web.Infrastructure.Extensions
{
    public static class GuidExtensions
    {
        public static bool HasValue(this Guid? source)
        {
            return source != null && source.Value != Guid.Empty;
        }

        public static string ToShortId(this Guid source)
        {
            long i = 1;

            foreach (var b in source.ToByteArray())
            {
                i *= ((int)b + 1);
            }

            return $"{i - DateTime.Now.Ticks:x}";
        }
    }
}