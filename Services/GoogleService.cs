using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Abstractions;
using Microsoft.Identity.Web;
using SocialNexusClient.ViewModels.Google;

namespace SocialNexusClient.Services
{
    public class GoogleService : Controller, IGoogleService
    {
        private readonly ILogger<GoogleService> _logger;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly HttpClient _httpClient;
        private readonly ITokenAcquisition _tokenAcquisition;
        private readonly IAuthorizationHeaderProvider _authorizationHeaderProvider;
        private IWebHostEnvironment CurrentEnvironment;

        public GoogleService(
            ITokenAcquisition tokenAcquisition,
            HttpClient httpClient,
            IConfiguration configuration,
            IHttpContextAccessor contextAccessor,
            ILogger<GoogleService> logger,
            IAuthorizationHeaderProvider authorizationHeaderProvider,
            IWebHostEnvironment env
        )
        {
            _logger = logger;
            _httpClient = httpClient;
            _tokenAcquisition = tokenAcquisition;
            _contextAccessor = contextAccessor;
            _authorizationHeaderProvider = authorizationHeaderProvider;
            CurrentEnvironment = env;
        }

        public async Task<IActionResult> GetReportTypesAsync()
        {
            await RequestAccessToken("https://www.googleapis.com/auth/yt-analytics.readonly");
            var response = await this._httpClient.GetAsync(
                $"https://youtubereporting.googleapis.com/v1/reportTypes"
            );

            _logger.LogCritical(response.ToString());
            response.EnsureSuccessStatusCode();

            var cnt = await response.Content.ReadFromJsonAsync<List<ReportTypeViewModel>>();

            if (cnt is List<ReportTypeViewModel>)
            {
                // foreach (Product p in cnt) { }
                return Ok(cnt);
            }

            return Problem();
        }

        [Authorize]
        [AuthorizeForScopes(
            Scopes = new[] { "https://www.googleapis.com/auth/yt-analytics.readonly" }
        )]
        public async Task RequestAccessToken(string scope)
        {
            try
            {
                string accessToken =
                    await _authorizationHeaderProvider.CreateAuthorizationHeaderForUserAsync(
                        new[] { scope }
                    );
                // var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(
                //     new[] { scope },
                // );

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
