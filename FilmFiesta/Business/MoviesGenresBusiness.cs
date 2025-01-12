using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class MoviesGenresBusiness : IMoviesGenresBusiness
    {
        private readonly IMoviesGenresRepository _moviesGenresRepository;
        private readonly IGenresRepository _genresRepository;

        public MoviesGenresBusiness(IMoviesGenresRepository moviesGenresRepository, IGenresRepository genresRepository)
        {
            _moviesGenresRepository = moviesGenresRepository;
            _genresRepository = genresRepository;
        }

        public async Task Add(List<string> MoviesGenres, long MovieId)
        {
            foreach (string movieGenre in MoviesGenres)
            {
                long genreId = _genresRepository.GetIdGenre(movieGenre);
                _ = await _moviesGenresRepository.Insert(new Movie_Genre()
                {
                    Movie_ID = MovieId,
                    Genre_ID = genreId,
                });
            }
        }

        public async Task<bool> DeleteByMovie(long movieId)
        {
            List<Movie_Genre> movies_genres = _moviesGenresRepository.GetAllByMovie(movieId);

            try
            {
                foreach (Movie_Genre movieGenre in movies_genres)
                {
                    _ = await _moviesGenresRepository.Delete(movieGenre.Id);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
    }
}
