using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Features.TestApis
{
    public class TimingWrappedResult<T>
    {
        public long TotalDuration { get; set; }
        public T Data { get; set; }
    }

    public class Stats
    {
        public long TotalDuration { get; set; }
        public string Name { get; set; }
    }
}