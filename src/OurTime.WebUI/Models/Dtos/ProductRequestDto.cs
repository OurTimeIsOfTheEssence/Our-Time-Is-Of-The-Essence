namespace OurTime.WebUI.Models.Dtos
{
    public class ProductRequestDto
    {
        public long ProductId { get; set; }
        public string Name { get; set; } = default!;
        public string Category { get; set; } = default!;
        public TagDto[] Tags { get; set; } = Array.Empty<TagDto>();
        public int CustomerId { get; set; }
    }
}
