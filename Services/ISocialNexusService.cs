using Microsoft.AspNetCore.Mvc;
using SocialNexusClient.ViewModels;

namespace SocialNexusClient.Services
{
    public interface ISocialNexusService
    {
        Task<IActionResult> GetTimeAsync();
        Task<IActionResult> GetProductsAsync();
        Task<ProductViewModel> PostProductAsync(ProductViewModel ps);
        Task<IActionResult> DeleteProductAsync(int id);
    }
}
