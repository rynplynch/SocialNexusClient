using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Services;
using SocialNexusClient.ViewModels;

namespace SocialNexusClient.Controllers
{
    public class ShopController : Controller
    {
        private ISocialNexusService _socialNexusService;
        private readonly ILogger<ShopController> _logger;

        public ShopController(
            ISocialNexusService socialNexusService,
            ILogger<ShopController> logger
        )
        {
            _socialNexusService = socialNexusService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductViewModel p)
        {
            string? objectIdIsNameIdentifier = User.FindFirstValue(ClaimTypes.NameIdentifier);
            p.OwnerId = objectIdIsNameIdentifier!;

            if (!ModelState.IsValid)
                return View("Index");

            await _socialNexusService.PostProductAsync(p);
            return View("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _socialNexusService.DeleteProductAsync(id);
            return View("Index");
        }
    }
}
