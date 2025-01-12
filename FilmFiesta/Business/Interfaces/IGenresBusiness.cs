using FilmFiesta.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business.Interfaces
{
    public interface IGenresBusiness
    {
        public IEnumerable<Genre> GetAll();
        bool GenreAllExist(IEnumerable<string> genres);
        bool GenreExists(string genre);
        Task<Genre> GenreAdd(string genre);
        Task<Genre> GenreUpdate(long genreId, string genreName);
        Task<bool> GenreRemove(long genreId);
    }
}
