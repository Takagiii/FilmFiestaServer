using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class GenresBusiness : IGenresBusiness
    {
        private readonly IGenresRepository _genresRepository;
        private readonly IMapper _mapper;

        public GenresBusiness(IGenresRepository genresRepository, IMapper mapper)
        {
            _genresRepository = genresRepository;
            _mapper = mapper;
        }

        public IEnumerable<Genre> GetAll()
        {
            return _genresRepository.GetAll();
        }

        public bool GenreAllExist(IEnumerable<string> genres)
        {
            IEnumerable<string> allGenres = _mapper.Map<IEnumerable<string>>(GetAll().Select(existingGenre => existingGenre.Name));
            return genres.Count() == 0 || genres.All(genre => allGenres.Any(existingGenre => string.Equals(existingGenre, genre, StringComparison.CurrentCultureIgnoreCase)));
        }

        public bool GenreExists(string genre)
        {
            return _genresRepository.GetGenre(genre) != null;
        }

        public async Task<Genre> GenreAdd(string genreName)
        {
            Genre genre = new()
            {
                Name = genreName,
            };

            return await _genresRepository.Insert(genre);
        }

        public async Task<bool> GenreRemove(long genreId)
        {
            return await _genresRepository.Delete(genreId);
        }

        public async Task<Genre> GenreUpdate(long genreId, string genreName)
        {
            Genre genre = new()
            {
                Id = genreId,
                Name = genreName,
            };
            return await _genresRepository.Update(genre);
        }
    }
}
