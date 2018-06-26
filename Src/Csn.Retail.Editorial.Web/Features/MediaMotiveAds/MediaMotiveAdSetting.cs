using System;
using System.Collections.Generic;
using System.Linq;

namespace Csn.Retail.Editorial.Web.Features.MediaMotiveAds
{
    public class MediaMotiveAdSetting
    {
        public MediaMotiveAdType Description { get; set; }
        public bool DataKruxRequired { get; set; }
        public AdSize AdSize { get; set; }
        public List<string> NotSupportedArticleTypes { get; set; }
    }

    public enum MediaMotiveAdType
    {
        Leaderboard,
        MREC,
        TEADS,
        Tracking,
        Banner
    }

    public enum AdSize
    {
        [Dimension(728, 90)]
        Leaderboard,
        [Dimension(628, 60)]
        BodyFullWidth,
        [Dimension(300, 250)]
        MediumRectangle,
        [Dimension(300, 250)]
        [Dimension(300, 600)]
        MediumOrLargeRectangle,
        [Dimension(300, 600)]
        LargeRectangle,
        [Dimension(300, 100)]
        SmallRectangle,
        [Dimension(550, 90)]
        Block550X90,
        [Dimension(1, 1)]
        Hidden,
        [Dimension(550, 309)]
        Block550X309,
        [Dimension(430, 78)]
        ListingItemNarrow,
        [Dimension(300, 470)]
        MediumLong,
        [Dimension(640, 100)]
        LongRectangle,
        [Dimension(640, 250)]
        Sponsored640X250,
        [Dimension(350, 1115)]
        Gutter,
        [Dimension(628, 150)]
        BodyFullWidth150Height,
        [Dimension(1200, 100)]
        Block1200X100
    }

    public class Dimension
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class DimensionAttribute : Attribute
    {
        public DimensionAttribute()
        {
        }

        public DimensionAttribute(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Width { get; set; }
        public int Height { get; set; }
    }

    public static class DimensionExtensions
    {
        public static IEnumerable<Dimension> Dimensions<TEnum>(this TEnum source) where TEnum : struct
        {
            var memberInfo = typeof(TEnum).GetMember(source.ToString()).FirstOrDefault();

            if (memberInfo == null) return Enumerable.Empty<Dimension>();

            var attributes = memberInfo.GetCustomAttributes(typeof(DimensionAttribute), false);

            return from DimensionAttribute dimensionAttr in attributes
                select new Dimension
                {
                    Width = dimensionAttr.Width,
                    Height = dimensionAttr.Height
                };
        }
    }
}