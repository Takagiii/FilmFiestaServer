using FilmFiestaFront.Helpers;
using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using System.Text.Json;

namespace FilmFiestaFront.Pages.Movie
{
    public class UpdateMovieModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieListModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        [BindProperty]
        public MovieUpdateRequest MovieUpdateRequest { get; set; }

        public List<SelectListItem> GenreSelectListItems { get; set; }
        public List<SelectListItem> StatutSelectListItems { get; set; }

        public UpdateMovieModel(
            IHttpClientFactory httpClientFactory,
            ILogger<MovieListModel> logger,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (HttpContext.Session.GetString("JWToken") == null)
                return RedirectToPage("/Login");
            GenreSelectListItems = EnumHelper.GetGenreSelectListItems();
            StatutSelectListItems = EnumHelper.GetStatutSelectListItems();
            try
            {
                var id = HttpContext.GetRouteData().Values["id"];
                var response = await _httpClient.GetAsync($"Movie/{id}");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                MovieUpdateRequest = JsonSerializer.Deserialize<MovieUpdateRequest>(json, _jsonSerializerOptions);
                if (MovieUpdateRequest == null)
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
                _logger.LogError(e, "Error fetching movie details from API.");
            }
            catch (JsonException e)
            {
                _logger.LogError(e, "Error deserializing the movie data.");
            }
            return RedirectToPage("/Error");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var routeDataId = HttpContext.GetRouteData().Values["id"];
                if (routeDataId != null && long.TryParse(routeDataId.ToString(), out long id))
                {
                    MovieUpdateRequest.Id = id;
                }
                StringContent jsonRequest = new(JsonSerializer.Serialize(MovieUpdateRequest, _jsonSerializerOptions), 
                    Encoding.UTF8, "application/json");
                var response = await _httpClient.PatchAsync($"Movie", jsonRequest);
                response.EnsureSuccessStatusCode();
                return RedirectToPage("/Movie/List");
            }
            catch (HttpRequestException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    return RedirectToPage("/Login");
                }
                _logger.LogError(e, "Error adding movie from API.");
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error.");
            }
            return RedirectToPage("/Error");
        }
    }
}
