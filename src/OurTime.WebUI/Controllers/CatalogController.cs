using Microsoft.AspNetCore.Mvc;
using OurTime.Application.Services.Interfaces;
using OurTime.Domain.Entities;
using OurTime.WebUI.Models.Catalog;
using OurTime.WebUI.Models.Dtos;

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
    [HttpPost]
    public async Task<IActionResult> AddProduct(WatchDto watch)
    {
        var _watch = new Watch(
            name: watch.Name,
            imageUrl: watch.ImageUrl,
            price: watch.Price,
            model: watch.Model,
            description: watch.Description
        );

        await _catalogService.AddWatchAsync(_watch);
        return RedirectToAction("Index", "Catalog");
    }


    [HttpPost]
    public async Task<IActionResult> DeleteProduct(int Id)
    {
        Watch watch = await _catalogService.GetWatchByIdAsync(Id);
        if (watch == null)
        {
            return NotFound();
        }
        await _catalogService.DeleteWatchAsync(watch);

        return RedirectToAction("Index", "Catalog");
    }   
}