using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Models.Entities;

namespace OurTime.WebUI.Models.ViewModels
{
    public class WatchWithReviewsViewModel
    {
        public Watch Watch { get; set; } = default!;
        public IEnumerable<ReviewDto> Reviews { get; set; } = Enumerable.Empty<ReviewDto>();
    }
}