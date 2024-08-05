using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using SocialNexusClient.ViewModels;

namespace SocialNexusClient.Services
{
    public class SocialNexusService : Controller, ISocialNexusService
    {
        private readonly ILogger<SocialNexusService> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly string _CreateProductScope = string.Empty;
        private readonly string _SocialNexusScope = string.Empty;
        private readonly string _SociaNexusBaseAddress = string.Empty;
        private readonly ITokenAcquisition _tokenAcquisition;
        private IWebHostEnvironment CurrentEnvironment;

        public SocialNexusService(
            ITokenAcquisition tokenAcquisition,
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            ILogger<SocialNexusService> logger,
            IWebHostEnvironment env
        )
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _contextAccessor = contextAccessor;
            _CreateProductScope = configuration["SocialNexusService:Scopes:CreateProduct"]!;
            _SocialNexusScope = configuration["SocialNexusService:SocialNexusScope"]!;
            CurrentEnvironment = env;

            // could change base address for development purposes
            // TODO: research dev-cert? Having an issues with establishing ssl connection
            if (CurrentEnvironment.IsDevelopment())
                _SociaNexusBaseAddress = configuration[
                    // changing to live production api for now
                    // "SocialNexusService:SocialNexusBaseAddressDev"
                    "SocialNexusService:SocialNexusBaseAddress"
                ]!;
            else
                _SociaNexusBaseAddress = configuration[
                    "SocialNexusService:SocialNexusBaseAddress"
                ]!;
        }

        public async Task<IActionResult> GetTimeAsync()
        {
            var response = await this._httpClient.GetAsync($"{_SociaNexusBaseAddress}time");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<TimeViewModel>();

            if (json is TimeViewModel)
                return Ok(json);

            return View("Error");
        }

        public async Task<IActionResult> GetProductsAsync()
        {
            // await PrepareAuthenticatedClient();
            var response = await this._httpClient.GetAsync($"{_SociaNexusBaseAddress}products");

            response.EnsureSuccessStatusCode();

            var cnt = await response.Content.ReadFromJsonAsync<List<ProductViewModel>>();

            if (cnt is List<ProductViewModel>)
            {
                // foreach (Product p in cnt) { }
                return Ok(cnt);
            }

            return View("Error");
        }

        [HttpPost]
        public async Task<ProductViewModel> PostProductAsync(ProductViewModel ps)
        {
            await RequestAccessToken(_CreateProductScope);

            var psJson = System.Text.Json.JsonSerializer.Serialize(ps);

            var requestContent = new StringContent(psJson, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(
                $"{_SociaNexusBaseAddress}products",
                requestContent
            );
            response.EnsureSuccessStatusCode();
            // var content = await response.Content.ReadAsStringAsync();
            var cnt = await response.Content.ReadFromJsonAsync<ProductViewModel>();
            // var createdProduct = JsonSerializer.Deserialize<Product>(content);

            // HttpContext ctx =
            // var response = await this._httpClient.PostAsync($"{_SociaNexusBaseAddress}products", );

            // response.EnsureSuccessStatusCode();

            // var cnt = await response.Content.ReadFromJsonAsync<ProductViewModel>();

            return null!;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProductAsync(int id)
        {
            await RequestAccessToken(_CreateProductScope);
            var response = await _httpClient.DeleteAsync(
                $"{_SociaNexusBaseAddress}products?id={id}"
            );

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadFromJsonAsync<ProductViewModel>();

            if (json is ProductViewModel)
                return Ok(json);

            return Problem();
        }

        [Authorize]
        public async Task RequestAccessToken(string scope)
        {
            try
            {
                var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(
                    new[] { scope }
                );
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                    "Bearer",
                    accessToken
                );
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json")
                );
            }
            catch (Exception e)
            {
                Console.WriteLine("THE ERROR: " + e);
            }
        }
    }
}
