using Csn.SimpleCqrs;

namespace Csn.Retail.Editorial.Web.Features.Landing.Carousel
{
    public class CarouselQuery : IQuery
    {
        public string Q { get; set; }
        public int Offset { get; set; }
        public int Limit { get; set; }
        public string Sort { get; set; }
    }
}