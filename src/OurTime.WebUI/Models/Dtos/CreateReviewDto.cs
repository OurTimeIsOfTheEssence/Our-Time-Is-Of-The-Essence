using System.ComponentModel.DataAnnotations;

namespace OurTime.WebUI.Models.Dtos
{
    public class CreateReviewDto
    {
        [Required]
        public string ReviewerName { get; set; } = string.Empty;

        [Required, Range(1, 5)]
        public int Rating { get; set; }

        [Required, StringLength(1000)]
        public string Text { get; set; } = string.Empty;
    }
}