using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FilmFiestaFront.Pages
{
    public class UserListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserListModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public List<UserDetail>? userDetails { get; set; }

        public UserListModel(
            IHttpClientFactory httpClientFactory, 
            ILogger<UserListModel> logger, 
            JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<IActionResult> OnGet()
        {
            if (HttpContext.Session.GetString("JWToken") == null)
                return RedirectToPage("/Login");
            try
            {
                var response = await _httpClient.GetAsync("User");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                userDetails = JsonSerializer.Deserialize<List<UserDetail>>(json, _jsonSerializerOptions);

                if (userDetails == null)
                {
                    _logger.LogWarning("Deserialization returned null.");
                }
                return Page();
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage("/Login");
                }
                _logger.LogError(e, "Error fetching users from API.");
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing the users data.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error.");
            }
            return RedirectToPage("/Error");
        }
    }
}
