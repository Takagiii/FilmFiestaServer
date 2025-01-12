using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IMoviesVotesRepository : IRepository<TMovies_Votes, Movie_Vote>
    {
        Movie_Vote UserVote(long userId);
        List<MovieByVote> MovieByVotes();
        Task<bool> RemoveAllVotesForMovie(long movieId);
    }
}
