using FilmFiesta.Business.Interfaces;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieOfTheDayUpdateController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        public MovieOfTheDayUpdateController(IMoviesBusiness moviesBusiness)
        {
            _moviesBusiness = moviesBusiness;
        }

        /// <summary>
        /// Updates the movie of the day
        /// </summary>
        /// <response code="200">Movie of the day successfully updated</response>
        /// <response code="404">Movie of the day not found</response>
        /// <response code="500">Movie of the day unsuccessfully updated</response>
        /// <returns>The new movie of the day.</returns>
        [Authorize]
        [HttpPatch("Free")]
        public async Task<IActionResult> UpdateMovieOfTheDay([FromBody] MovieOfTheDayUpdateRequest request)
        {
            Dbo.Movie movie = await _moviesBusiness.UpdateMovieOfTheDay(request.movieId);
            return movie == null ? StatusCode(404, "Movie of the day not found") : StatusCode(200, movie);
        }
    }
}
