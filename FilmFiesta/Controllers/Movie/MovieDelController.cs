using AutoMapper;
using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieDelController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        private readonly IMoviesGenresBusiness _moviesGenresBusiness;
        private readonly IMapper _mapper;
        public MovieDelController(IMoviesBusiness moviesBusiness, IMoviesGenresBusiness moviesGenresBusiness, IMapper mapper)
        {
            _moviesBusiness = moviesBusiness;
            _moviesGenresBusiness = moviesGenresBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Remove a movie
        /// </summary>
        /// <response code="200">Movie successfully removed</response>
        /// <response code="400">Movie could not be removed</response>
        /// <returns>The success.</returns>
        [Authorize]
        [HttpDelete("{movieId}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] long movieId)
        {
            try
            {
                bool success = await _moviesBusiness.Delete(movieId);
                return success ? StatusCode(200, success) : StatusCode(400, "Movie could not be removed");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
