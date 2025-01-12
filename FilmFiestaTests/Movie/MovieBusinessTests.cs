using AutoMapper;
using FilmFiesta.Business;
using FilmFiesta.Business.Interfaces;
using FilmFiesta.DataAccess;
using FilmFiesta.DataAccess.Interfaces;
using FilmFiesta.Models;
using Moq;

namespace FilmFiestaTests.Movie
{
    public class MovieBusinessTests
    {
        public required Mock<IMoviesRepository> _moviesRepository = new();
        public required Mock<IMoviesGenresBusiness> _moviesGenresBusiness = new();
        public required Mock<IMoviesVotesBusiness> _moviesVotesBusiness = new();
        private readonly MapperConfiguration mapperConfig = new(cfg =>
        {
            cfg.AddProfile<AutomapperProfiles>();
        });

        [Fact]
        public void GetMovieOfTheDayTest()
        {
            _ = _moviesRepository.Setup(m => m.GetMovieOfTheDayDetail()).Returns(new MovieDetail());
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            MovieDetail result = moviesBusiness.GetMovieOfTheDayDetail();
            Assert.NotNull(result);
        }

        [Fact]
        public void GetTest()
        {
            int id = 2;
            _ = _moviesRepository.Setup(m => m.GetDetail(id)).Returns(new MovieDetail());
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            MovieDetail result = moviesBusiness.Get(id);
            Assert.NotNull(result);
        }

        [Fact]
        public void GetAllBySearchTest()
        {
            List<string> genres = [];
            List<StatutType> status = [];
            string search = "";

            _ = _moviesRepository.Setup(m => m.GetAllBySearch(genres, status, search)).Returns([]);
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            IEnumerable<MoviePreview> result = moviesBusiness.GetAllBySearch(genres, status, search);
            Assert.NotNull(result);
        }

        [Fact]
        public async void AddTest()
        {
            FilmFiesta.Dbo.Movie movie = new();
            _ = _moviesRepository.Setup(m => m.Insert(movie)).ReturnsAsync(new FilmFiesta.Dbo.Movie());
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            FilmFiesta.Dbo.Movie result = await moviesBusiness.Add(movie);
            Assert.NotNull(result);
        }

        [Fact]
        public async void DeleteTest()
        {
            long movieId = 2;
            _ = _moviesRepository.Setup(m => m.Delete(movieId)).ReturnsAsync(true);
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            bool result = await moviesBusiness.Delete(movieId);
            Assert.True(result);
        }

        [Fact]
        public async void UpdateTest()
        {
            FilmFiesta.Dbo.Movie movie = new();
            _ = _moviesRepository.Setup(m => m.Update(movie)).ReturnsAsync(new FilmFiesta.Dbo.Movie());
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            FilmFiesta.Dbo.Movie result = await moviesBusiness.Update(movie);
            Assert.NotNull(result);
        }

        [Fact]
        public async void UpdateNullTest()
        {
            FilmFiesta.Dbo.Movie movie = new();
            _ = _moviesRepository.Setup(m => m.Update(movie)).ReturnsAsync(value: null);
            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            FilmFiesta.Dbo.Movie result = await moviesBusiness.Update(movie);
            Assert.Null(result);
        }

        [Fact]
        public async void UpdateMovieOfTheDayTest()
        {
            long movieId = 2;
            FilmFiesta.Dbo.Movie movieOfTheDay = new();
            _ = _moviesRepository.Setup(m => m.GetMovie(movieId)).Returns(movieOfTheDay).Callback(() =>
            {
                movieOfTheDay = new FilmFiesta.Dbo.Movie();
            });

            _ = _moviesRepository.Setup(m => m.GetMovieOfTheDay()).Returns(new FilmFiesta.Dbo.Movie());

            _ = _moviesRepository.Setup(m => m.Update(movieOfTheDay)).ReturnsAsync(new FilmFiesta.Dbo.Movie()
            {
                Titre = "FinalResult"
            });

            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            FilmFiesta.Dbo.Movie result = await moviesBusiness.UpdateMovieOfTheDay(movieId);
            Assert.Equal("FinalResult", result.Titre);
            Assert.Equal(StatutType.Free, result.Statut);
        }

        [Fact]
        public async void UpdateMovieOfTheDayNonExistantTest()
        {
            long movieId = 86;
            _ = _moviesRepository.Setup(m => m.GetMovie(movieId)).Returns(value: null);

            MoviesBusiness moviesBusiness = new(_moviesRepository.Object, _moviesGenresBusiness.Object, _moviesVotesBusiness.Object, null);

            FilmFiesta.Dbo.Movie result = await moviesBusiness.UpdateMovieOfTheDay(movieId);
            Assert.Null(result);
        }
    }
}
