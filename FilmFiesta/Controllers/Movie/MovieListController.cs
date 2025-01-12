using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.Models;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieListController : Controller
    {
        private readonly IMoviesBusiness _moviesBusiness;
        private readonly IGenresBusiness _genresBusiness;
        private readonly IMapper _mapper;
        public MovieListController(IMoviesBusiness moviesBusiness, IGenresBusiness genresBusiness, IMapper mapper)
        {
            _moviesBusiness = moviesBusiness;
            _genresBusiness = genresBusiness;
            _mapper = mapper;
        }

        /// <summary>
        /// Gets all movies
        /// </summary>
        /// <response code="200">Movies successfully obtained</response>
        /// <response code="404">Movies not found</response>
        /// <response code="500">Movies unsuccessfully obtained</response>
        /// <returns>The movies.</returns>
        [Authorize]
        [HttpGet]
        public IActionResult GetAllMovies([FromQuery] MovieListRequest search)
        {
            try
            {
                List<string> genres = [];
                if (_genresBusiness.GenreAllExist(search.Genres) == true)
                {
                    genres = search.Genres;
                }

                IEnumerable<StatutType> status = _mapper.Map<IEnumerable<StatutType>>(search.Status);
                string research = search.Research;
                IEnumerable<MoviePreview> movies = _moviesBusiness.GetAllBySearch(genres, status, research);
                return movies.Count() <= 0 ? StatusCode(404, "Movies not found") : StatusCode(200, movies);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
