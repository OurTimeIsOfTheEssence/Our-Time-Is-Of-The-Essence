// Controllers/WatchesMvcController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Models.Entities;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Services;

namespace OurTime.WebUI.Controllers
{
    public class WatchesMvcController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ReviewApiService _reviews;

        public WatchesMvcController(ApplicationDbContext db, ReviewApiService reviews)
        {
            _db = db;
            _reviews = reviews;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var watches = await _db.Watches.ToListAsync();
            return View(watches);
        }

        // GET /WatchesMvc/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var watch = await _db.Watches.FindAsync(id);
            if (watch == null)
                return NotFound();

            var recs = (await _reviews.GetReviewsAsync(id)).ToList();
            var vm = new ProductReviewsViewModel
            {
                Product = watch,
                Reviews = recs,
                NewReview = new CreateReviewDto()
            };

            return View(vm);
        }

        // POST /WatchesMvc/Details/5
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, ProductReviewsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Reviews = (await _reviews.GetReviewsAsync(id)).ToList();
                return View(vm);
            }

            var created = await _reviews.PostReviewAsync(id, vm.NewReview);
            if (created == null)
            {
                ModelState.AddModelError("", "Kunde inte spara recensionen.");
                vm.Reviews = (await _reviews.GetReviewsAsync(id)).ToList();
                return View(vm);
            }

            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
