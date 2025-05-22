namespace OurTime.WebUI.Models.Dtos
{
    public class ReviewDto
    {
        public long ReviewId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime ReviewDate { get; set; }
    }
}