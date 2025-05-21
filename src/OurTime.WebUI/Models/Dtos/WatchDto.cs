using System.ComponentModel.DataAnnotations;

namespace OurTime.WebUI.Models.Dtos
{
    public class WatchDto
    {
        // För PUT (uppdatering) sätts Id, annars 0
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
    }
}
