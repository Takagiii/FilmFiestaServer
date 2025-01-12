using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.DataAccess.Repositories
{
    public class MoviesGenresRepository : Repository<TMovies_Genres, Movie_Genre>, IMoviesGenresRepository
    {
        public MoviesGenresRepository(FilmFiestaContext context, ILogger<MoviesGenresRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public List<Movie_Genre> GetAllByMovie(long movieId)
        {
            return _mapper.Map<List<Movie_Genre>>(_context.TMoviesGenres.Where(movie_genre => movie_genre.Movie_ID == movieId).ToList());
        }
    }
}
