using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Controllers;
using SocialNexusClient.Services;
using SocialNexusClient.ViewModels.Google;

namespace SocialNexusClient.ViewComponents
{
    public class GoogleViewComponent : ViewComponent
    {
        private IGoogleService _googleService;
        private readonly ILogger<GoogleViewComponent> _logger;

        public GoogleViewComponent(
            IGoogleService googleService,
            ILogger<GoogleViewComponent> logger
        )
        {
            _googleService = googleService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            IActionResult res = await _googleService.GetReportTypesAsync();

            if (res is OkObjectResult)
            {
                OkObjectResult ok = (OkObjectResult)res;
                return View(ok.Value!);
            }
            else
            {
                return View("Error");
            }
            // ReportTypeViewModel rt = new ReportTypeViewModel();
            // rt.Id = "channel_annotations_a1";
            // rt.name = "Annotations";
            // List<ReportTypeViewModel> l = new List<ReportTypeViewModel>();
            // l.Add(rt);
        }
    }
}
