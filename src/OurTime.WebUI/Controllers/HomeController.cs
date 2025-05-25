using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Services;

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
        public IActionResult ShowCart() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Watches() => View();

        [HttpGet]
        public async Task<IActionResult> Reviews(int productId)
        {
            var product = await _db.Watches.FindAsync(productId);
            if (product == null)
                return NotFound();

            if (product.ExternalProductId == null)
            {
                var dto = new ProductRequestDto
                {
                    ProductId = 0,
                    Name = product.Name,
                    Category = product.Model,
                    Tags = new[]
                    {
                        new TagDto { Id = 3, Name = "watch" },
                        new TagDto { Id = 4, Name = "timepiece" }
                    },
                    CustomerId = 1
                };

                var newId = await _reviews.RegisterAndReturnIdAsync(dto);
                if (newId == null)
                    return StatusCode(502, "Could not register product with ReviewEngine.");

                product.ExternalProductId = newId.Value;
                await _db.SaveChangesAsync();
            }

            long extId = product.ExternalProductId.Value;
            var reviews = (await _reviews.GetReviewsAsync((int)extId)).ToList();

            var vm = new ProductReviewsViewModel
            {
                Product = product,
                Reviews = reviews,
                NewReview = new CreateReviewDto()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reviews(int productId, ProductReviewsViewModel vm)
        {
            var product = await _db.Watches.FindAsync(productId);
            if (product == null)
                return NotFound();

            if (product.ExternalProductId == null)
                return BadRequest("Product missing ExternalProductId.");

            long extId = product.ExternalProductId.Value;

            if (!ModelState.IsValid)
            {
                vm.Reviews = (await _reviews.GetReviewsAsync((int)extId)).ToList();
                vm.Product = product;
                return View(vm);
            }

            var created = await _reviews.PostReviewAsync((int)extId, vm.NewReview);
            if (created == null)
            {
                ModelState.AddModelError("", "Could not save your review.");
                vm.Reviews = (await _reviews.GetReviewsAsync((int)extId)).ToList();
                vm.Product = product;
                return View(vm);
            }

            return RedirectToAction(nameof(Reviews), new { productId });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
            => View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
