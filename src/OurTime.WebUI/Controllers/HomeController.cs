using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OurTime.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly ReviewApiService _reviews;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db,
            ReviewApiService reviews)
        {
            _logger = logger;
            _db = db;
            _reviews = reviews;
        }

        public IActionResult Index() => View();
        public IActionResult Privacy() => View();
        public IActionResult Watches() => View();
        public IActionResult ShowCart() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();

        // GET /Home/Reviews?productId=123
        // GET /Home/Reviews?productId=123
        [HttpGet]
        public async Task<IActionResult> Reviews(int productId)
        {
            // 1) Load the watch from our own database
            var product = await _db.Watches.FindAsync(productId);
            if (product == null) return NotFound();

            // 2) Ensure the product exists in the Review Engine
            var requestDto = new ProductRequestDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Category = product.Model,
                Tags = new[] { "watch", "timepiece" }
            };
            await _reviews.RegisterProductAsync(requestDto);

            // 3) Fetch its reviews
            var reviews = (await _reviews.GetReviewsAsync(productId)).ToList();

            // 4) Build and return the view model
            var vm = new ProductReviewsViewModel
            {
                Product = product,
                Reviews = reviews,
                NewReview = new CreateReviewDto()
            };
            return View(vm);  // will use Views/Home/Reviews.cshtml
        }

        // POST /Home/Reviews
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reviews(int productId, ProductReviewsViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                // Re-load reviews and product if validation fails
                vm.Reviews = (await _reviews.GetReviewsAsync(productId)).ToList();
                vm.Product = await _db.Watches.FindAsync(productId) ?? throw new InvalidOperationException();
                return View(vm);
            }

            // Submit the new review
            var created = await _reviews.PostReviewAsync(productId, vm.NewReview);
            if (created == null)
            {
                ModelState.AddModelError("", "Could not save your review.");
                vm.Reviews = (await _reviews.GetReviewsAsync(productId)).ToList();
                vm.Product = await _db.Watches.FindAsync(productId) ?? throw new InvalidOperationException();
                return View(vm);
            }

            // Redirect back to GET to avoid duplicate posts
            return RedirectToAction(nameof(Reviews), new { productId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}