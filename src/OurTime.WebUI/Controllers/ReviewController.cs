using Microsoft.AspNetCore.Mvc;

namespace OurTime.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReviewController : ControllerBase
    {
        // POST api/review/save
        [HttpPost("save")]
        public IActionResult Save([FromBody] ReviewDto dto)
        {
            // Här kan du antingen spara till egen databas
            // eller anropa den andra klassens API med HttpClient
            return Ok(new { success = true });
        }
    }
}
