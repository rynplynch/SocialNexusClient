using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Services;
using SocialNexusClient.ViewModels;

namespace SocialNexusClient.Controllers
{
    public class GoogleController : Controller
    {
        // private IGoogleService _googleService;
        private readonly ILogger<GoogleController> _logger;

        public GoogleController(
            // IGoogleService googleService,
            ILogger<GoogleController> logger
        )
        {
            // _googleService = googleService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            // _logger.LogCritical(objectIdIsNameIdentifier);
            return View();
        }

        // [HttpPost]
        // [Authorize]
        // [ValidateAntiForgeryToken]
        // public async Task<IActionResult> CreateProduct(ProductViewModel p)
        // {
        //     string? objectIdIsNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //     p.OwnerId = objectIdIsNameIdentifier!;

        //     if (!ModelState.IsValid)
        //         return View("Index");

        //     await _socialNexusService.PostProductAsync(p);
        //     return View("Index");
        // }

        // public async Task<IActionResult> DeleteProduct(int id)
        // {
        //     await _socialNexusService.DeleteProductAsync(id);
        //     return View("Index");
        // }
    }
}
