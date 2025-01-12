using AutoMapper;
using FilmFiesta.DataAccess.EfModels;
using FilmFiesta.Dbo;
using FilmFiesta.Requests.Movies;

namespace FilmFiesta.DataAccess
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            _ = CreateMap<TGenres, Genre>().ReverseMap();

            _ = CreateMap<TUsers, User>().ReverseMap();

            _ = CreateMap<TSubscriptions, Subscription>().ReverseMap();

            _ = CreateMap<TMovies, Movie>().ReverseMap();

            _ = CreateMap<TMovies_Genres, Movie_Genre>().ReverseMap();

            _ = CreateMap<MovieAddRequest, Movie>().ReverseMap();

            _ = CreateMap<TMovies_Votes, Movie_Vote>().ReverseMap();

            _ = CreateMap<MovieByVoteView, MovieByVote>().ReverseMap();
        }
    }
}
