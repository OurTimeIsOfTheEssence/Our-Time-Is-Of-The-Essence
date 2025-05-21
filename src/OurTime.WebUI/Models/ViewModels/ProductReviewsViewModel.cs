using OurTime.WebUI.Models.Entities;
using OurTime.WebUI.Models.Dtos;

namespace OurTime.WebUI.Models.ViewModels
{
    public class ProductReviewsViewModel
    {
        // Här använder du er Watch-klass som "Product"
        public Watch Product { get; set; } = default!;

        // Listan av recensioner du hämtar via externa API:t
        public List<ReviewDto> Reviews { get; set; } = new();
    }
}
