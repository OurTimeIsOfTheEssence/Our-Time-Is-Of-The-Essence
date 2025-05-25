using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models;
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

        public IActionResult Index()
        {
            var watches = new List<WatchViewModel>
            {
                new WatchViewModel
                {
                    Name = "OT ASP.NET",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/Asp.net.png",
                    Price = "12,999 SEK",
                    Description = "A sleek, dark timepiece designed for developers and tech lovers.",
                    Features = new List<string> {
                        "Material: Stainless Steel",
                        "Movement: Quartz",
                        "Water Resistance: 30 meters"
                    }
                },
                new WatchViewModel
                {
                    Name = "OT Terra",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/Terra.png",
                    Price = "29,999 SEK",
                    Description = "Titanium case and automatic movement – built for adventurers.",
                    Features = new List<string> {
                        "Titanium shell", "Automatic movement", "Luminous dials"
                    }
                },
                new WatchViewModel
                {
                    Name = "OT Rose A1",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/Rose A1.png",
                    Price = "39,999 SEK",
                    Description = "Luxury rose gold with fine leather strap.",
                    Features = new List<string> {
                        "Rose gold case", "Elegant leather", "Swiss quartz"
                    }
                },
                new WatchViewModel
                {
                    Name = "OT Lynx A2",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/Lynx A2.png",
                    Price = "24,499 SEK",
                    Description = "Bold luminous hands, sporty yet elegant.",
                    Features = new List<string> {
                        "Sport design", "Luminous hands", "Waterproof"
                    }
                },
                new WatchViewModel
                {
                    Name = "OT Bohemian",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/Bohemian.png",
                    Price = "10,999 SEK",
                    Description = "Artistic and charming design for creative souls.",
                    Features = new List<string> {
                        "Creative dial", "Slim fit", "Matte finish"
                    }
                },
                new WatchViewModel
                {
                    Name = "OT Vector",
                    ImageUrl = "https://storageaccountblobb.blob.core.windows.net/images/VectorV1.png",
                    Price = "89,999 SEK",
                    Description = "The OT Vector is a masterpiece of engineering, combining lightweight titanium with precision Swiss movement.",
                    Features = new List<string> {
                        "Material: Titanium case and bracelet",
                        "Movement: Swiss automatic movement",
                        "Crystal: Scratch-resistant sapphire crystal",
                        "Water Resistance: 100 meters (10 ATM)",
                        "Special Features: Luminous hands and markers, date display"
                    }
                }
            };

            var connStr = Environment.GetEnvironmentVariable("STATICWATCH_CONNECTION");
            if (!string.IsNullOrWhiteSpace(connStr))
            {
                using var connection = new SqlConnection(connStr);
                connection.Open();

                var command = new SqlCommand("SELECT Name, ImageUrl, Price, Description, Features FROM StaticWatches", connection);
                using var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var features = new List<string>();
                    var raw = reader["Features"]?.ToString();
                    if (!string.IsNullOrWhiteSpace(raw))
                    {
                        features = raw.Split(';', StringSplitOptions.RemoveEmptyEntries)
                                      .Select(f => f.Trim()).ToList();
                    }

                    watches.Add(new WatchViewModel
                    {
                        Name = reader["Name"].ToString(),
                        ImageUrl = reader["ImageUrl"].ToString(),
                        Price = string.Format("{0:N0} SEK", reader["Price"]),
                        Description = reader["Description"].ToString(),
                        Features = features
                    });
                }
            }

            return View(watches);
        }

        public IActionResult Privacy() => View();
        public IActionResult ShowCart() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Watches() => View();

        [HttpGet]
        public async Task<IActionResult> Reviews(int productId)
        {
            var product = await _db.Watches.FindAsync(productId);
            if (product == null) return NotFound();

            if (product.ExternalProductId == null)
            {
                var dto = new ProductRequestDto
                {
                    ProductId = 0,
                    Name = product.Name,
                    Category = product.Model,
                    Tags = new[] {
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

            var extId = product.ExternalProductId.Value;
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
            if (product == null) return NotFound();
            if (product.ExternalProductId == null)
                return BadRequest("Product missing ExternalProductId.");

            var extId = product.ExternalProductId.Value;

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
