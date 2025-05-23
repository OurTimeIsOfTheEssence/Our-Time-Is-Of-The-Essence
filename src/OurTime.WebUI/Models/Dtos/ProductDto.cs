namespace OurTime.WebUI.Models.Dtos
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public double Rating { get; set; }
        public TagDto[] Tags { get; set; } = Array.Empty<TagDto>();
        public DateTime CreateDate { get; set; }
        public ReviewDto[] Reviews { get; set; } = Array.Empty<ReviewDto>();
    }
}
