using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FilmFiestaFront.Pages
{
    public class MovieDetailsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieDetailsModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public MovieDetail? movieDetail;
        public string? genres;

        public MovieDetailsModel(
            IHttpClientFactory httpClientFactory,
            ILogger<MovieDetailsModel> logger,
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
                var id = HttpContext.GetRouteData().Values["id"];
                var response = await _httpClient.GetAsync($"Movie/{id}");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                movieDetail = JsonSerializer.Deserialize<MovieDetail>(json, _jsonSerializerOptions);

                if (movieDetail == null)
                {
                    _logger.LogWarning("Deserialization returned null.");
                }

                genres = ListGenres();
                return Page();
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage("/Login");
                }
                _logger.LogError(e, "Error fetching movie details from API.");
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

        public async Task<IActionResult> OnPostDelete(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/Movie/{id}");
                response.EnsureSuccessStatusCode();
                return RedirectToPage("/Movie/List");
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage("/Login");
                }
                _logger.LogError(e, "Error fetching movie details from API.");
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing the movie data.");
            }
            return RedirectToPage("/Error");
        }

        private string ListGenres()
        {
            string result = "";
            if (movieDetail == null) return result;

            for (int i = 0; i < movieDetail.Genres.Count; i++)
            {
                result += movieDetail.Genres[i];
                if (i < movieDetail.Genres.Count - 1) 
                    result += "&middot;";
            }

            return result;
        }
    }
}
