using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Listings
{
    public class GetListingsQuery : IQuery
    {
        private string _seoFragment = string.Empty;

        public string Q { get; set; }
        public int Offset { get; set; }
        public string Sort { get; set; }
        public string Keywords { get; set; }

        public string SeoFragment
        {
            get => _seoFragment;

            set {
                if (string.IsNullOrEmpty(_seoFragment))
                {
                    _seoFragment = $"/{value}";
                }
            }
        }
    }
}