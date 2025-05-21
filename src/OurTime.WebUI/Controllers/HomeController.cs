using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OurTime.WebUI.Data;
using OurTime.WebUI.Models.ViewModels;
using OurTime.WebUI.Models.Dtos;
using OurTime.WebUI.Services;
using System.Diagnostics;

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
        public async Task<IActionResult> Reviews(int productId)
        {
            // 1) Hämta klockan från er egen databas
            var product = await _db.Watches
                                   .FirstOrDefaultAsync(w => w.Id == productId);
            if (product == null)
                return NotFound();

            // 2) Hämta recensioner via externa API:t
            var fetched = await _reviews.GetReviewsAsync(productId);

            // 3) Paketera i en ViewModel
            var vm = new ProductReviewsViewModel
            {
                Product = product,
                Reviews = fetched.ToList()
            };

            // 4) Skicka till vy
            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() =>
            View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
    }
}
