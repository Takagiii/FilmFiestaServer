using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmFiesta.DataAccess.Repositories
{
    public class MoviesRepository : Repository<TMovies, Movie>, IMoviesRepository
    {
        public MoviesRepository(FilmFiestaContext context, ILogger<MoviesRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public bool IsVotedMovie(long movieId)
        {
            Movie movie = _mapper.Map<Movie>(_context.TMovies
                .Where(movie => movie.Statut == StatutType.Voted.ToString())
                .FirstOrDefault((TMovies movie) => movie.Id == movieId));
            return movie != null;
        }

        public MovieDetail GetMovieOfTheDayDetail()
        {
            return (from query_movies in _context.TMovies
                    where query_movies.Statut == StatutType.Free.ToString()
                    join query_movies_genres in _context.TMoviesGenres on query_movies.Id equals query_movies_genres.Movie_ID
                    join query_genres in _context.TGenres on query_movies_genres.Genre_ID equals query_genres.Id
                    group query_genres.Name by new
                    {
                        query_movies.Id,
                        query_movies.Titre,
                        query_movies.Realisateur,
                        query_movies.Affiche,
                        query_movies.Duree,
                        query_movies.Date_de_sortie,
                        query_movies.Description,
                        query_movies.EN_Description,
                        query_movies.Video,
                        query_movies.Statut
                    } into g
                    select new MovieDetail()
                    {
                        Id = g.Key.Id,
                        Titre = g.Key.Titre,
                        Realisateur = g.Key.Realisateur,
                        Affiche = g.Key.Affiche,
                        Duree = g.Key.Duree,
                        Date_de_sortie = g.Key.Date_de_sortie,
                        Description = g.Key.Description,
                        EN_Description = g.Key.EN_Description,
                        Video = g.Key.Video,
                        Genres = g.ToList(),
                        Statut = g.Key.Statut,
                    })
                   .FirstOrDefault();
        }

        public Movie GetMovieOfTheDay()
        {
            return _mapper.Map<Movie>(_context.TMovies.Where(movie => movie.Statut == "Free").FirstOrDefault());
        }

        public MovieDetail GetDetail(long id)
        {
            return (from query_movies in _context.TMovies
                    where query_movies.Id == id
                    join query_movies_genres in _context.TMoviesGenres on query_movies.Id equals query_movies_genres.Movie_ID
                    join query_genres in _context.TGenres on query_movies_genres.Genre_ID equals query_genres.Id
                    group query_genres.Name by new
                    {
                        query_movies.Id,
                        query_movies.Titre,
                        query_movies.Realisateur,
                        query_movies.Affiche,
                        query_movies.Duree,
                        query_movies.Date_de_sortie,
                        query_movies.Description,
                        query_movies.EN_Description,
                        query_movies.Video,
                        query_movies.Statut
                    } into g
                    select new MovieDetail()
                    {
                        Id = g.Key.Id,
                        Titre = g.Key.Titre,
                        Realisateur = g.Key.Realisateur,
                        Affiche = g.Key.Affiche,
                        Duree = g.Key.Duree,
                        Date_de_sortie = g.Key.Date_de_sortie,
                        Description = g.Key.Description,
                        EN_Description = g.Key.EN_Description,
                        Video = g.Key.Video,
                        Genres = g.ToList(),
                        Statut = g.Key.Statut,
                    })
                   .FirstOrDefault();
        }

        public Movie GetMovie(long movieId)
        {
            return _mapper.Map<Movie>(_context.TMovies.Where(movie => movie.Id == movieId).FirstOrDefault());
        }

        // pas utilisé ni update pour l'instant
        //public Movie GetByName(string name)
        //{
        //    return _mapper.Map<Movie>(_context.TMovies.Find((TMovies movie) => movie.Titre.Contains(name)));
        //}

        //// pas utilisé ni update pour l'instant
        //public IEnumerable<object> GetAll()
        //{
        //    return _context.TMovies
        //            .Select(movie => new { movie.Id, movie.Titre, movie.Realisateur, movie.Affiche })
        //            .ToList();
        //}

        public IEnumerable<MoviePreview> GetAllBySearch(IEnumerable<string> genres, IEnumerable<StatutType> statusTypes, string search)
        {
            IEnumerable<string> status = _mapper.Map<IEnumerable<string>>(statusTypes);

            return (from query_movies in _context.TMovies
                    join query_movies_genres in _context.TMoviesGenres on query_movies.Id equals query_movies_genres.Movie_ID
                    join query_genres in _context.TGenres on query_movies_genres.Genre_ID equals query_genres.Id
                    where genres.Count() == 0 || genres.Contains(query_genres.Name)
                    where query_movies.Titre.Contains(search)
                    where status.Contains(query_movies.Statut)
                    select new MoviePreview
                    {
                        Id = query_movies.Id,
                        Titre = query_movies.Titre,
                        Realisateur = query_movies.Realisateur,
                        Affiche = query_movies.Affiche,
                    })
                    .Distinct()
                    .ToList();
        }
    }
}
