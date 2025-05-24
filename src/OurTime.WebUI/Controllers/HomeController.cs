using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Models;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Services;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;


namespace OurTime.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly ReviewApiService _reviews;
        private readonly IConfiguration _configuration;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db,
            ReviewApiService reviews,
            IConfiguration configuration)
        {
            _logger = logger;
            _db = db;
            _reviews = reviews;
            _configuration = configuration;
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

            // ✅ Ladda in klockor från StaticWatches i databasen
            var connectionString = _configuration.GetConnectionString("StaticWatchConnection");
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                var command = new SqlCommand("SELECT * FROM StaticWatches", connection);
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var featuresRaw = reader["Features"]?.ToString();
                    var features = featuresRaw != null
                        ? featuresRaw.Split(';', StringSplitOptions.RemoveEmptyEntries).Select(f => f.Trim()).ToList()
                        : new List<string>();

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
        public IActionResult Watches() => View();
        public IActionResult ShowCart() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();

        [HttpGet]
        public async Task<IActionResult> Reviews(int productId)
        {
            var product = await _db.Watches.FindAsync(productId);
            if (product == null) return NotFound();

            var requestDto = new ProductRequestDto
            {
                ProductId = product.Id,
                Name = product.Name,
                Category = product.Model,
                Tags = new[] { "watch", "timepiece" }
            };
            await _reviews.RegisterProductAsync(requestDto);

            var reviews = (await _reviews.GetReviewsAsync(productId)).ToList();

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
            if (!ModelState.IsValid)
            {
                vm.Reviews = (await _reviews.GetReviewsAsync(productId)).ToList();
                vm.Product = await _db.Watches.FindAsync(productId) ?? throw new InvalidOperationException();
                return View(vm);
            }

            var created = await _reviews.PostReviewAsync(productId, vm.NewReview);
            if (created == null)
            {
                ModelState.AddModelError("", "Could not save your review.");
                vm.Reviews = (await _reviews.GetReviewsAsync(productId)).ToList();
                vm.Product = await _db.Watches.FindAsync(productId) ?? throw new InvalidOperationException();
                return View(vm);
            }

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