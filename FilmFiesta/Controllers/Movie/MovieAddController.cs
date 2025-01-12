using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieAddController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        private readonly IMoviesGenresBusiness _moviesGenresBusiness;
        private readonly IGenresBusiness _genresBusiness;
        private readonly IMapper _mapper;
        public MovieAddController(IMoviesBusiness moviesBusiness, IMoviesGenresBusiness moviesGenresBusiness, IGenresBusiness genresBusiness, IMapper mapper)
        {
            _moviesBusiness = moviesBusiness;
            _moviesGenresBusiness = moviesGenresBusiness;
            _genresBusiness = genresBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Add a movie
        /// </summary>
        /// <response code="200">Movie successfully added</response>
        /// <response code="400">Movie could not be added</response>
        /// <returns>The movie.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieAddRequest request)
        {
            try
            {
                if (_genresBusiness.GenreAllExist(request.Genres) == false)
                {
                    return StatusCode(400, "Unexistant genre");
                }

                if (request.Genres.Count() == 0)
                {
                    return StatusCode(400, "At least one Genre is needed");
                }

                Dbo.Movie mappedMovie = _mapper.Map<Dbo.Movie>(request);
                Dbo.Movie movie = await _moviesBusiness.Add(mappedMovie);

                if (movie == null)
                {
                    return StatusCode(400, "Movie could not be added");
                }
                else
                {
                    await _moviesGenresBusiness.Add(request.Genres, movie.Id);
                    return StatusCode(200, movie);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
