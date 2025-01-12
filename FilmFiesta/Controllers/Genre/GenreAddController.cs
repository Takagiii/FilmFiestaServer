using FilmFiesta.Business.Interfaces;
using FilmFiesta.Requests.Genres;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Genre
{
    [ApiController]
    [Route("Genre")]
    public class GenreAddController : Controller
    {
        private readonly IGenresBusiness _genresBusiness;
        public GenreAddController(IGenresBusiness genresBusiness)
        {
            _genresBusiness = genresBusiness;
        }

        /// <summary>
        /// Add a genre
        /// </summary>
        /// <response code="200">Genre successfully added</response>
        /// <response code="500">Genre unsuccessfully added</response>
        /// <returns>The genre.</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> AddGenre(GenreAddRequest request)
        {
            try
            {
                Dbo.Genre genre = await _genresBusiness.GenreAdd(request.genreName);
                return genre == null ? StatusCode(500, "Genre could not be added") : StatusCode(200, genre);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
