using FilmFiesta.Dbo;
using FilmFiesta.Models;
using FilmFiesta.Requests.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface IMoviesBusiness
    {
        MovieDetail GetMovieOfTheDayDetail();
        Task<Movie> UpdateMovieOfTheDay(long movieId);
        MovieDetail Get(long id);
        Movie MovieSetInfo(MovieUpdateRequest request);

        IEnumerable<MoviePreview> GetAllBySearch(IEnumerable<string> genres, IEnumerable<StatutType> statut, string search);
        Task<Movie> Add(Movie movie);
        Task<bool> Delete(long movieId);
        Task<Movie> Update(Movie movie);
    }
}
