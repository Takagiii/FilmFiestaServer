using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface IMoviesGenresBusiness
    {
        Task Add(List<string> MoviesGenres, long MovieId);
        Task<bool> DeleteByMovie(long movieId);
    }
}
