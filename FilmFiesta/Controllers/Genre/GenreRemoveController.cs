using FilmFiesta.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Genre
{
    [ApiController]
    [Route("Genre")]
    public class GenreRemoveController : Controller
    {
        private readonly IGenresBusiness _genresBusiness;
        public GenreRemoveController(IGenresBusiness genresBusiness)
        {
            _genresBusiness = genresBusiness;
        }

        /// <summary>
        /// Remove a genre
        /// </summary>
        /// <response code="200">Genre successfully removed</response>
        /// <response code="400">Genre unsuccessfully removed</response>
        /// <returns>The success.</returns>
        [Authorize]
        [HttpDelete("{genreId}")]
        public async Task<IActionResult> RemoveGenre(long genreId)
        {
            try
            {
                bool success = await _genresBusiness.GenreRemove(genreId);
                return success ? StatusCode(200, success) : StatusCode(400, "Genre could not be removed");
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
