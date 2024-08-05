using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Controllers;
using SocialNexusClient.Services;

namespace SocialNexusClient.ViewComponents
{
    public class ProductsViewComponent : ViewComponent
    {
        private ISocialNexusService _socialNexusService;
        private readonly ILogger<HomeController> _logger;

        public ProductsViewComponent(
            ISocialNexusService socialNexusService,
            ILogger<HomeController> logger
        )
        {
            _socialNexusService = socialNexusService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IActionResult res = await _socialNexusService.GetProductsAsync();

            if (res is OkObjectResult)
            {
                OkObjectResult ok = (OkObjectResult)res;
                return View(ok.Value!);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
