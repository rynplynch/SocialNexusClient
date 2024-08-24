using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.ViewModels;

namespace SocialNexusClient.Services
{
    public interface IGoogleService
    {
        Task<IActionResult> GetReportTypesAsync();
    }
}
