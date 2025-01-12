using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.DataAccess.Repositories
{
    public class GenresRepository : Repository<TGenres, Genre>, IGenresRepository
    {
        public GenresRepository(FilmFiestaContext context, ILogger<GenresRepository> logger, IMapper mapper) : base(context, logger, mapper) { }

        public IEnumerable<Genre> GetAll()
        {
            return _mapper.Map<List<Genre>>(_context.TGenres);
        }

        public long GetIdGenre(string genreName)
        {
            return _context.TGenres.FirstOrDefault(genre => genre.Name == genreName).Id;
        }

        public Genre GetGenre(string genreName)
        {
            return _mapper.Map<Genre>(_context.TGenres.FirstOrDefault(genre => genre.Name == genreName));
        }
    }
}
