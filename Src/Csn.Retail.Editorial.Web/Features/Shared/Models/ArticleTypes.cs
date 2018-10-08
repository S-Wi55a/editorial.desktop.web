using System.ComponentModel.DataAnnotations;

namespace Csn.Retail.Editorial.Web.Features.Shared.Models
{
    public enum ArticleType
    {
        News,
        Review,
        Video,
        Sponsored,
        Advice,
        Features,
        Reviews,
        Carpool,
        Actualidad,
        [Display(Name = "Las Mejores Compras")]
        LasMejoresCompras,
        [Display(Name = "Compra de Auto")]
        CompraDeAuto,
        Comparativas,
        Consejos,
        Pruebas
    }
}