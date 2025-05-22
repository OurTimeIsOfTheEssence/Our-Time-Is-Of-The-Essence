using Microsoft.AspNetCore.Mvc;
using OurTime.WebUI.Models;
using System.Diagnostics;

namespace OurTime.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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

            return View(watches);
        }

        public IActionResult Watches() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View();
        public IActionResult Privacy() => View();
        public IActionResult ShowCart() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
