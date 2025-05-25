namespace OurTime.WebUI.Models.Dtos
{
    public class ProductDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = "";
        public string Category { get; set; } = "";
        public string[] Tags { get; set; } = Array.Empty<string>();
        public DateTime CreatedDate { get; set; }
        public ReviewDto[] Reviews { get; set; } = Array.Empty<ReviewDto>();
    }
}