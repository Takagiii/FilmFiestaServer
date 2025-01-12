using FilmFiesta.Business.Interfaces;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieUpdateController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        public MovieUpdateController(IMoviesBusiness moviesBusiness)
        {
            _moviesBusiness = moviesBusiness;
        }

        /// <summary>
        /// Updates a movie
        /// </summary>
        /// <response code="200">Movie successfully updated</response>
        /// <response code="400">Movie could not be updated</response>
        /// <returns>The updated movie.</returns>
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieUpdateRequest request)
        {
            try
            {
                Dbo.Movie currentMovie = _moviesBusiness.MovieSetInfo(request);

                if (currentMovie == null)
                {
                    return StatusCode(400, "Wrong movie id");
                }

                Dbo.Movie updatedMovie = await _moviesBusiness.Update(currentMovie);
                return updatedMovie == null ? StatusCode(400, "Movie could not be updated") : StatusCode(200, updatedMovie);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
