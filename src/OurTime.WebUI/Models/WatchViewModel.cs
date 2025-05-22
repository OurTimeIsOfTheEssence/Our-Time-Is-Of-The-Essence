namespace OurTime.WebUI.Models
{
    public class WatchViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Price { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> Features { get; set; } = new();
    }
}
