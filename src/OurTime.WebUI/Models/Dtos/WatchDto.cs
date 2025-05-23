using System.ComponentModel.DataAnnotations;

namespace OurTime.WebUI.Models.Dtos
{
    public class WatchDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Model { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        public string Category { get; set; } = string.Empty;

        public string[] Tags { get; set; } = Array.Empty<string>();

        public DateTime CreatedDate { get; set; }
    }
}