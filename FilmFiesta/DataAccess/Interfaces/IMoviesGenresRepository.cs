using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IMoviesGenresRepository : IRepository<TMovies_Genres, Movie_Genre>
    {
        //Movie_Genre GetByMovie(long movieId);
        //Movie_Genre GetByGenre(long genreId);
        List<Movie_Genre> GetAllByMovie(long movieId);
    }
}
