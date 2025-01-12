using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.Controllers.Genre
{
    [ApiController]
    [Route("Genre")]
    public class GenreListController : Controller
    {
        private readonly IGenresBusiness _genresBusiness;
        public GenreListController(IGenresBusiness genresBusiness)
        {
            _genresBusiness = genresBusiness;
        }

        /// <summary>
        /// Gets all genres
        /// </summary>
        /// <response code="200">Genres successfully obtained</response>
        /// <response code="404">Genres not found</response>
        /// <response code="500">Genres unsuccessfully obtained</response>
        /// <returns>The genres.</returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetAllGenres()
        {
            try
            {
                IEnumerable<Dbo.Genre> genres = _genresBusiness.GetAll();
                return genres.Count() <= 0 ? StatusCode(404, "Genres not found") : StatusCode(200, genres);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
