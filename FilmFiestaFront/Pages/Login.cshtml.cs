using FilmFiestaFront.Models;
using FilmFiestaFront.Pages.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;

namespace FilmFiestaFront.Pages
{
    public class LoginModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieListModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        [BindProperty]
        public LoginRequest? LoginRequest { get; set; }
        public string? ErrorMsg { get; set; }

        public LoginModel(
            IHttpClientFactory httpClientFactory,
            ILogger<MovieListModel> logger,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }
    
        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                StringContent jsonRequest = new(JsonSerializer.Serialize(LoginRequest), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Login/Admin", jsonRequest);
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    AuthResponse? authResponse = JsonSerializer.Deserialize<AuthResponse>(jsonResponse, _jsonSerializerOptions);
                    HttpContext.Session.SetString("JWToken", authResponse.Token);
                    return RedirectToPage("/Index");
                }
                else
                {
                    ErrorMsg = "Incorrect email or password.";
                }
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing the auth response data.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error.");
            }
            return Page();
        }
    }
}
