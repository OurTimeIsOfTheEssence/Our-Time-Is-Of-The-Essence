using Microsoft.AspNetCore.Mvc;
using OurTime.Application.Services.Interfaces;
using OurTime.WebUI.Models.Catalog;

namespace OurTime.WebUI.Controllers;

public class CatalogController : Controller
{
    private readonly ICatalogService _catalogService;

    public CatalogController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    // GET: Catalog
    public async Task<IActionResult> Index()
    {
        try
        {
            // Get all products from the service
            var watches = await _catalogService.GetAllWatchesAsync();

            // Map domain entities to view models
            var productViewModels = watches.Select(p => new ProductCardViewModel
            {
                Name = p.Name,
                Model = p.Model,
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Description = p.Description,
                Price = p.Price,
                
            }).ToList();

            // Create the product catalog view model
            var viewModel = new ProductCatalogViewModel
            {
                FeaturedProducts = productViewModels
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            // In a real application, you should use a proper logging framework
            Console.WriteLine($"Error in ProductCatalog: {ex.Message}");

            // Show an error message to the user
            ViewBag.ErrorMessage = "An error occurred while loading products. Please try again later.";
            return View("Error");
        }
    }

    // GET: Store/Details/5
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            // Get the specific product from the service
            var watch = await _catalogService.GetWatchByIdAsync(id);

            // Return 404 if product not found
            if (watch is null)
            {
                return NotFound();
            }

            // Map domain entity to view model
            var viewModel = new ProductDetailsViewModel
            {
                Id = watch.Id,
                Name = watch.Name,
                ImageUrl = watch.ImageUrl,
                Description = watch.Description,
                Price = watch.Price,

            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            // Log the exception
            Console.WriteLine($"Error in ProductDetails: {ex.Message}");

            // Show an error message to the user
            ViewBag.ErrorMessage = "An error occurred while loading the product. Please try again later.";
            return View("Error");
        }
    }
}