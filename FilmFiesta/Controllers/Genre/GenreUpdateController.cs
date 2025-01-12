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
    public class GenreUpdateController : Controller
    {
        private readonly IGenresBusiness _genresBusiness;
        public GenreUpdateController(IGenresBusiness genresBusiness)
        {
            _genresBusiness = genresBusiness;
        }

        /// <summary>
        /// Updates a genre
        /// </summary>
        /// <response code="200">Genre successfully updated</response>
        /// <response code="500">Genre unsuccessfully updated</response>
        /// <returns>The genre.</returns>
        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> UpdateGenre(GenreUpdateRequest request)
        {
            try
            {
                Dbo.Genre genre = await _genresBusiness.GenreUpdate(request.genreId, request.genreName);
                return genre == null ? StatusCode(500, "Genre could not be updated") : StatusCode(200, genre);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
