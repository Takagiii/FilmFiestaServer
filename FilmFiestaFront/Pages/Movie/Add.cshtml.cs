using FilmFiestaFront.Models;
using FilmFiestaFront.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FilmFiestaFront.Pages.Movie
{
    public class AddMovieModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieListModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        [BindProperty]
        public MovieAddRequest MovieAddRequest { get; set; }
        public List<SelectListItem> GenreSelectListItems { get; set; }
        public List<SelectListItem> StatutSelectListItems { get; set; }
        public string ErrorMsg { get; set; }

        public AddMovieModel(
            IHttpClientFactory httpClientFactory,
            ILogger<MovieListModel> logger,
            JsonSerializerOptions jsonSerializerOptions)
        {
            _httpClient = httpClientFactory.CreateClient("client");
            _logger = logger;
            _jsonSerializerOptions = jsonSerializerOptions;
        }

        public IActionResult OnGet()
        {
            if (HttpContext.Session.GetString("JWToken") == null)
                return RedirectToPage("/Login");
            GenreSelectListItems = EnumHelper.GetGenreSelectListItems();
            StatutSelectListItems = EnumHelper.GetStatutSelectListItems();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            try
            {
                StringContent jsonRequest = new(JsonSerializer.Serialize(MovieAddRequest, _jsonSerializerOptions), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("Movie", jsonRequest);
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
