namespace OurTime.WebUI.Models.Dtos;

public class ReviewDto
{
    public int Id { get; set; }
    public int JinProductId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime Date { get; set; } 
}
