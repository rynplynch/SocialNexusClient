using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Controllers;
using SocialNexusClient.Services;

namespace SocialNexusClient.ViewComponents
{
    public class TimeViewComponent : ViewComponent
    {
        private ISocialNexusService _socialNexusService;
        private readonly ILogger<TimeViewComponent> _logger;

        public TimeViewComponent(
            ISocialNexusService socialNexusService,
            ILogger<TimeViewComponent> logger
        )
        {
            _socialNexusService = socialNexusService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IActionResult time = await _socialNexusService.GetTimeAsync();

            if (time is OkObjectResult)
            {
                OkObjectResult res = (OkObjectResult)time;
                return View(res.Value!);
            }
            else
            {
                return View("Error");
            }
        }
    }
}
