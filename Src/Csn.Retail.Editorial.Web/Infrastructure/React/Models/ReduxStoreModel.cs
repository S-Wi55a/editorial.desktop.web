using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Csn.Retail.Editorial.Web.Infrastructure.React.Models
{
    public class ReduxStoreModel
    {
        public object State { get; set; }
        public string ReducerKey { get; set; }
        public string ReducerName { get; set; }
 
    }
}