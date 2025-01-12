using FilmFiestaFront.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace FilmFiestaFront.Pages
{
    public class StatsModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<MovieDetailsModel> _logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        public List<string> Labels { get; set; }
        public List<int?> Data { get; set; }

        public StatsModel(
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
                HttpResponseMessage response = await _httpClient.GetAsync($"Movie/Stat");
                response.EnsureSuccessStatusCode();
                string json = await response.Content.ReadAsStringAsync();
                List<MovieByVote>? moviesVotes = JsonSerializer.Deserialize<List<MovieByVote>>(json, _jsonSerializerOptions);

                if (moviesVotes == null)
                {
                    _logger.LogWarning("Deserialization returned null.");
                }

                Labels = [moviesVotes[0].Titre, moviesVotes[1].Titre];
                Data = [moviesVotes[0].VoteCount, moviesVotes[1].VoteCount];

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
            catch (Exception e)
            {
                _logger.LogError(e, "Unhandled error.");
            }
            return RedirectToPage("/Error");
        }
    }
}
