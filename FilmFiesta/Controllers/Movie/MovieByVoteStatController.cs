using FilmFiesta.Business.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieByVoteStatController : Controller
    {
        private readonly IMoviesVotesBusiness _moviesVotesBusiness;
        public MovieByVoteStatController(IMoviesVotesBusiness moviesVotesBusiness)
        {
            _moviesVotesBusiness = moviesVotesBusiness;
        }

        /// <summary>
        /// Gets the stats for the voted movies
        /// </summary>
        /// <response code="200">Movie stat successfully get</response>
        /// <response code="404">Movie stat not found</response>
        /// <response code="500">Movie stat unsuccessfully get</response>
        /// <returns>The movies stat.</returns>
        [Authorize]
        [HttpGet("Stat")]
        public IActionResult GetMovieByVoteStat()
        {
            List<MovieByVote> moviesStats = _moviesVotesBusiness.MovieByVotes();
            return moviesStats.Count == 0 ? StatusCode(404, "Movie stat") : StatusCode(200, moviesStats);
        }
    }
}
