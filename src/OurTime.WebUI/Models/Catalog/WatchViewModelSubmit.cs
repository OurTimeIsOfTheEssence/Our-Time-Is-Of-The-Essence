using System.ComponentModel.DataAnnotations;

namespace OurTime.WebUI.Models.Catalog;

public class WatchViewModelSubmit
{
    [Required]
    public string Name { get; set; }

    public string? Model { get; set; }

    public string? Description { get; set; }

    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public string? Gender { get; set; }

    public int Stock { get; set; }

    public bool IsCustom { get; set; }
}
