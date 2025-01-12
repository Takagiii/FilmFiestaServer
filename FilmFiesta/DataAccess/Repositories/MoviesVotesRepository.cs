using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Dbo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmFiesta.DataAccess.Repositories
{
    public class MoviesVotesRepository : Repository<TMovies_Votes, Movie_Vote>, IMoviesVotesRepository
    {
        public MoviesVotesRepository(FilmFiestaContext context, ILogger<MoviesVotesRepository> logger, IMapper mapper) : base(context, logger, mapper)
        {
        }

        public Movie_Vote UserVote(long userId)
        {
            return _mapper.Map<Movie_Vote>(_context.TMoviesVotes.Where(vote => vote.Iduser == userId).FirstOrDefault());
        }

        public List<MovieByVote> MovieByVotes()
        {
            return _mapper.Map<List<MovieByVote>>(_context.MovieByVotes.ToList());
        }

        public async Task<bool> RemoveAllVotesForMovie(long movieId)
        {
            try
            {
                List<TMovies_Votes> votesToDelete = await _context.TMoviesVotes
                    .Where(vote => vote.Idmovie == movieId)
                    .ToListAsync();

                _context.TMoviesVotes.RemoveRange(votesToDelete);

                _ = await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
