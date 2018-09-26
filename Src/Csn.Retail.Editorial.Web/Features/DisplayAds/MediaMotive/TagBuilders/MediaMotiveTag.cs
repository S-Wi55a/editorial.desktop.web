using System.Collections.Generic;

namespace Csn.Retail.Editorial.Web.Features.DisplayAds.MediaMotive.TagBuilders {
    public class MediaMotiveTag
    {
        private readonly List<string> _values = new List<string>();

        public MediaMotiveTag(string name, string value)
        {
            Name = name;
            _values.Add(value);
        }
        public MediaMotiveTag(string name, IEnumerable<string> values)
        {
            Name = name;
            _values.AddRange(values);
        }

        public string Name { get; }

        public string[] Values => _values.ToArray();
    }
}