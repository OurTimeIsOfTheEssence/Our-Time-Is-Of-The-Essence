namespace OurTime.WebUI.Models.Dtos
{
    public class ProductRequestDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string[] Tags { get; set; } = Array.Empty<string>();
        // ev. public DateTime CreatedDate { get; set; }
    }
}