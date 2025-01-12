using FilmFiesta.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface IMoviesVotesBusiness
    {
        Task<Movie_Vote> AddVote(long userId, long movieId);
        bool HasVoted(long userId);
        List<MovieByVote> MovieByVotes();

        Task<bool> RemoveAllVotesForMovie(long movieId);
    }
}
