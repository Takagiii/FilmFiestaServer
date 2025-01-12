using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using System.Collections.Generic;

namespace FilmFiesta.DataAccess.Interfaces
{
    public interface IGenresRepository : IRepository<TGenres, Genre>
    {
        IEnumerable<Genre> GetAll();
        long GetIdGenre(string genreName);
        Genre GetGenre(string genreName);
    }
}
