using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Models.Entities;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Services;

namespace OurTime.WebUI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WatchesController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        public WatchesController(ApplicationDbContext db) => _db = db;

        // GET api/watches
        [HttpGet]
        public async Task<ActionResult<IEnumerable<WatchDto>>> GetAll()
        {
            var list = await _db.Watches
                .Select(w => new WatchDto
                {
                    Id = w.Id,
                    Name = w.Name,
                    Model = w.Model,
                    Price = w.Price,
                    Description = w.Description,
                    ImageUrl = w.ImageUrl
                })
                .ToListAsync();

            return Ok(list);
        }

        // GET api/watches
        [HttpGet("{id}")]
        public async Task<ActionResult<WatchDto>> GetById(int id)
        {
            var w = await _db.Watches.FindAsync(id);
            if (w == null) return NotFound();

            return Ok(new WatchDto
            {
                Id = w.Id,
                Name = w.Name,
                Model = w.Model,
                Price = w.Price,
                Description = w.Description,
                ImageUrl = w.ImageUrl
            });
        }

        // POST api/watches
        [HttpPost]
        public async Task<ActionResult<WatchDto>> Create([FromBody] WatchDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var entity = new Watch
            {
                Name = dto.Name,
                Model = dto.Model,
                Price = dto.Price,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
            };

            _db.Watches.Add(entity);
            await _db.SaveChangesAsync();

            dto.Id = entity.Id;
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT api/watches
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] WatchDto dto)
        {
            if (id != dto.Id) return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var exists = await _db.Watches.AnyAsync(w => w.Id == id);
            if (!exists) return NotFound();

            var entity = new Watch
            {
                Id = dto.Id,
                Name = dto.Name,
                Model = dto.Model,
                Price = dto.Price,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl
            };

            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // DELETE api/watches
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var w = await _db.Watches.FindAsync(id);
            if (w == null) return NotFound();

            _db.Watches.Remove(w);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}