using FilmFiesta.Business.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace FilmFiesta.Controllers.Movie
{
    [ApiController]
    [Route("Movie")]
    public class MovieVoteController : Controller
    {
        private readonly IMoviesVotesBusiness _moviesVotesBusiness;
        public MovieVoteController(IMoviesVotesBusiness moviesVotesBusiness)
        {
            _moviesVotesBusiness = moviesVotesBusiness;
        }

        /// <summary>
        /// Add a vote for a movie
        /// </summary>
        /// <response code="200">Movie vote successfully added</response>
        /// <response code="400">Movie vote could not be added</response>
        /// <returns>The vote.</returns>
        [Authorize]
        [HttpPost("vote")]
        public async Task<IActionResult> AddMovieVote([FromBody] MovieVoteRequest request)
        {
            try
            {
                bool alreadyVoted = _moviesVotesBusiness.HasVoted(request.UserId);
                if (alreadyVoted)
                {
                    return StatusCode(403, "User already had voted");
                }
                Movie_Vote vote = await _moviesVotesBusiness.AddVote(request.UserId, request.MovieId);
                return vote == null ? StatusCode(400, "Vote could not be added") : StatusCode(200, vote);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
