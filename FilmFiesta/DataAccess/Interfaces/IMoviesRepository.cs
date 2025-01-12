using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IMoviesRepository : IRepository<TMovies, Movie>
    {
        bool IsVotedMovie(long movieId);
        MovieDetail GetMovieOfTheDayDetail();
        Movie GetMovieOfTheDay();

        MovieDetail GetDetail(long id);
        Movie GetMovie(long id);

        //Movie GetByName(string name);

        //IEnumerable<object> GetAll();

        IEnumerable<MoviePreview> GetAllBySearch(IEnumerable<string> genres, IEnumerable<StatutType> status, string search = "");
    }
}
