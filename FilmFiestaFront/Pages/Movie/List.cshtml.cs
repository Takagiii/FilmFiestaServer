using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FilmFiestaFront.Pages.Movie
{
    public class MovieListModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieListModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public List<MoviePreview>? Movies { get; set; }

        public MovieListModel(
            IHttpClientFactory httpClientFactory,
            ILogger<MovieListModel> logger,
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
                var response = await _httpClient.GetAsync("Movie");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                Movies = JsonSerializer.Deserialize<List<MoviePreview>>(json, _jsonSerializerOptions);

                if (Movies == null)
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
                _logger.LogError(e, "Error fetching movies from API.");
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing the movie data.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error.");
            }
            return RedirectToPage("/Error");
        }
    }
}
