using FilmFiesta.Business.Interfaces;
using FilmFiesta.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmFiesta.Controllers.Movie
{
    [Authorize]
    [ApiController]
    [Route("Movie")]
    public class MovieGetController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        public MovieGetController(IMoviesBusiness moviesBusiness)
        {
            _moviesBusiness = moviesBusiness;
        }

        /// <summary>
        /// Gets the movie
        /// </summary>
        /// <remarks>
        /// An id is needed
        /// </remarks>
        /// <response code="200">Movie successfully get</response>
        /// <response code="400">Bad request: the id cannot be negative or null</response>
        /// <response code="404">Movie not found</response>
        /// <response code="500">Movie unsuccessfully get</response>
        /// <returns>The movie.</returns>
        [HttpGet("{id}")]
        public IActionResult GetMovie(long id)
        {
            MovieDetail movie = _moviesBusiness.Get(id);
            return movie == null ? StatusCode(404, "Movie not found") : StatusCode(200, movie);
        }

        /// <summary>
        /// Gets the movie by name
        /// </summary>
        /// <remarks>
        /// A name is needed
        /// </remarks>
        /// <response code="200">Movie successfully get</response>
        /// <response code="400">Bad request: the name must be non null</response>
        /// <response code="404">Movie not found</response>
        /// <response code="500">Movie unsuccessfully get</response>
        /// <returns>The movie.</returns>
        //[HttpGet("{name}")]
        //public IActionResult GetMovieByName(string name)
        //{
        //    Dbo.Movie movie = _moviesBusiness.GetByName(name);
        //    return movie == null ? StatusCode(404, "Movie not found") : (IActionResult)StatusCode(200, movie);
        //}
    }
}
