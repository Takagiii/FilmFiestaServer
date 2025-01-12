using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class MoviesVotesBusiness : IMoviesVotesBusiness
    {
        private readonly IMoviesVotesRepository _moviesVotesRepository;
        private readonly IMoviesRepository _moviesRepository;

        public MoviesVotesBusiness(IMoviesVotesRepository moviesVotesRepository, IMoviesRepository moviesRepository)
        {
            _moviesVotesRepository = moviesVotesRepository;
            _moviesRepository = moviesRepository;
        }

        public async Task<Movie_Vote> AddVote(long userId, long movieId)
        {
            return _moviesRepository.IsVotedMovie(movieId) == true ?
                await _moviesVotesRepository.Insert(new Movie_Vote()
                {
                    Iduser = userId,
                    Idmovie = movieId,
                })
                : null;
        }

        public bool HasVoted(long userId)
        {
            return _moviesVotesRepository.UserVote(userId) != null;
        }

        public List<MovieByVote> MovieByVotes()
        {
            return _moviesVotesRepository.MovieByVotes();
        }

        public async Task<bool> RemoveAllVotesForMovie(long movieId)
        {
            return await _moviesVotesRepository.RemoveAllVotesForMovie(movieId);
        }
    }
}
