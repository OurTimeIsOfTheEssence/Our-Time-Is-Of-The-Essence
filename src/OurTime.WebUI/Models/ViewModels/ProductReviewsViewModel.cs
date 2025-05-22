using OurTime.WebUI.Models.Entities;
using OurTime.WebUI.Models.Dtos;

namespace OurTime.WebUI.Models.ViewModels
{
    public class ProductReviewsViewModel
    {
        public Watch Product { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public CreateReviewDto NewReview { get; set; } = new CreateReviewDto();
    }
}
