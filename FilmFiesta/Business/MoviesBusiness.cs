using AutoMapper;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using FilmFiesta.Requests.Movies;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FilmFiesta.Business
{
    public class MoviesBusiness : IMoviesBusiness
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly IMoviesGenresBusiness _moviesGenresBusiness;
        private readonly IMoviesVotesBusiness _moviesVotesBusiness;
        private readonly IMapper _mapper;

        public MoviesBusiness(IMoviesRepository moviesRepository, IMoviesGenresBusiness moviesGenresBusiness, IMoviesVotesBusiness moviesVotesBusiness, IMapper mapper)
        {
            _moviesRepository = moviesRepository;
            _moviesGenresBusiness = moviesGenresBusiness;
            _moviesVotesBusiness = moviesVotesBusiness;
            _mapper = mapper;
        }

        public MovieDetail GetMovieOfTheDayDetail()
        {
            return _moviesRepository.GetMovieOfTheDayDetail();
        }

        public async Task<Movie> UpdateMovieOfTheDay(long movieId)
        {
            // verify that the movie exists first
            Movie movie = _moviesRepository.GetMovie(movieId);
            if (movie == null)
            {
                return null;
            }

            // change the old movie of the day to paid
            Movie oldMovieOfTheDay = _moviesRepository.GetMovieOfTheDay();

            // not normal because there must be always one free movie
            if (oldMovieOfTheDay != null)
            {
                oldMovieOfTheDay.Statut = StatutType.Paid;
                _ = await _moviesRepository.Update(oldMovieOfTheDay);
            }

            // change the paid to movie of the day
            movie.Statut = StatutType.Free;
            Movie updatedMovie = await _moviesRepository.Update(movie);

            return updatedMovie;
        }

        public MovieDetail Get(long id)
        {
            return _moviesRepository.GetDetail(id);
        }

        public Movie MovieSetInfo(MovieUpdateRequest request)
        {
            MovieDetail currentMovie = Get(request.Id);

            return currentMovie == null ? null :

            new()
            {
                Id = request.Id,
                Titre = request.Titre ?? currentMovie.Titre,
                Realisateur = request.Realisateur ?? currentMovie.Realisateur,
                Description = request.Description ?? currentMovie.Description,
                EN_Description = request.EN_Description ?? currentMovie.EN_Description,
                Duree = request.Duree ?? currentMovie.Duree,
                Date_de_sortie = request.Date_de_sortie ?? currentMovie.Date_de_sortie,
                Video = request.Video ?? currentMovie.Video,
                Affiche = request.Affiche ?? currentMovie.Affiche,
                Statut = request.Statut != null ? _mapper.Map<StatutType>(request.Statut) : _mapper.Map<StatutType>(currentMovie.Statut)
            };
        }

        public IEnumerable<MoviePreview> GetAllBySearch(IEnumerable<string> genres, IEnumerable<StatutType> status, string search)
        {
            return _moviesRepository.GetAllBySearch(genres, status, search);
        }

        public async Task<Movie> Add(Movie movie)
        {
            return await _moviesRepository.Insert(movie);
        }

        public async Task<bool> Delete(long movieId)
        {
            // also remove the genres linked to the movie
            _ = await _moviesGenresBusiness.DeleteByMovie(movieId);

            // also remove the votes for the movie

            return await _moviesRepository.Delete(movieId);
        }

        public async Task<Movie> Update(Movie movieUpdate)
        {
            Movie currentMovie = _moviesRepository.GetMovie(movieUpdate.Id);

            if (currentMovie == null)
            {
                return null;
            }

            // we need to verify if the current statut (before the update) is a voted movie. If so, we need to remove all votes.
            if (movieUpdate.Statut != StatutType.Voted)
            {
                StatutType currentMovieStatut = currentMovie.Statut;
                if (currentMovieStatut == StatutType.Voted)
                {
                    _ = await _moviesVotesBusiness.RemoveAllVotesForMovie(movieUpdate.Id);
                }
            }

            if (movieUpdate.Statut == StatutType.Free)
            {
                _ = await UpdateMovieOfTheDay(movieUpdate.Id);
            }

            return await _moviesRepository.Update(movieUpdate);
        }
    }
}
