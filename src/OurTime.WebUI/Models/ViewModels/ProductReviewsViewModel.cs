using OurTime.WebUI.Models.Dtos;
using OurTime.Domain.Entities;

namespace OurTime.WebUI.Models.ViewModels
{
    public class ProductReviewsViewModel
    {
        public Watch Product { get; set; }
        public List<ReviewDto> Reviews { get; set; }
        public CreateReviewDto NewReview { get; set; } = new CreateReviewDto();
    }
}