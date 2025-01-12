using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieOfTheDayGetController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        public MovieOfTheDayGetController(IMoviesBusiness moviesBusiness)
        {
            _moviesBusiness = moviesBusiness;
        }

        /// <summary>
        /// Gets the movie of the day
        /// </summary>
        /// <response code="200">Movie of the day successfully get</response>
        /// <response code="404">Movie of the day not found</response>
        /// <response code="500">Movie of the day unsuccessfully get</response>
        /// <returns>The movie of the day.</returns>
        [AllowAnonymous]
        [HttpGet("Free")]
        public IActionResult GetMovieOfTheDay()
        {
            object movie = _moviesBusiness.GetMovieOfTheDayDetail();
            return movie == null ? StatusCode(404, "Movie of the day not found") : StatusCode(200, movie);
        }
    }
}
