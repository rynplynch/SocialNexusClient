using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.Models;

namespace SocialNexusClient.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [Authorize]
    public IActionResult Claims()
    {
        // extract the claims principal from the http context
        // our controller passively passes on this data to the view
        ClaimsPrincipal? p = HttpContext.User;

        // render the view stored in the Claims.cshtml file
        return View();
    }

    [Authorize]
    public IActionResult Identities()
    {
        // extract the identities claims principle
        IEnumerable<ClaimsIdentity> p = HttpContext.User.Identities;

        // render the Identities.cshtml view
        return View();
    }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(
            new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier }
        );
    }
}
