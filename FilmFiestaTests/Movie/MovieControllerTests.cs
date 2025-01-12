using FilmFiesta.Business.Interfaces;
using FilmFiesta.Controllers.Movie;
using FilmFiesta.Dbo;
using FilmFiesta.Models;
using FilmFiesta.Requests.Movies;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilmFiestaTests.Movie
{
    public class MovieControllerTests
    {
        public Mock<IMoviesBusiness> _moviesBusiness = new();
        public Mock<IMoviesVotesBusiness> _moviesVoteBusiness = new();

        // Movie Get controller
        [Fact]
        public void GetTest()
        {
            long id = 1;
            _ = _moviesBusiness.Setup(m => m.Get(id)).Returns(new MovieDetail());

            MovieGetController controller = new(_moviesBusiness.Object);
            IActionResult result = controller.GetMovie(id);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }


        [Fact]
        public void GetNullTest()
        {
            long id = 1;
            _ = _moviesBusiness.Setup(m => m.Get(id)).Returns(value: null);

            MovieGetController controller = new(_moviesBusiness.Object);
            IActionResult result = controller.GetMovie(id);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(404, okResult?.StatusCode);
        }


        // Movie of the day controller
        [Fact]
        public void GetMovieOfTheDayTest()
        {
            _ = _moviesBusiness.Setup(m => m.GetMovieOfTheDayDetail()).Returns(new MovieDetail());

            MovieOfTheDayGetController controller = new(_moviesBusiness.Object);
            IActionResult result = controller.GetMovieOfTheDay();
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }


        [Fact]
        public void GetMovieOfTheDayNullTest()
        {
            _ = _moviesBusiness.Setup(m => m.GetMovieOfTheDayDetail()).Returns(value: null);

            MovieOfTheDayGetController controller = new(_moviesBusiness.Object);
            IActionResult result = controller.GetMovieOfTheDay();
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(404, okResult?.StatusCode);
        }


        // MovieVoteController

        [Fact]
        public async void GetMovieVoteAddTest()
        {
            long userId = 1;
            long movieId = 1;
            MovieVoteRequest request = new()
            {
                UserId = userId,
                MovieId = movieId,
            };

            _ = _moviesVoteBusiness.Setup(m => m.HasVoted(request.MovieId)).Returns(false);
            _ = _moviesVoteBusiness.Setup(m => m.AddVote(request.UserId, request.MovieId)).ReturnsAsync(new Movie_Vote());

            MovieVoteController controller = new(_moviesVoteBusiness.Object);
            IActionResult result = await controller.AddMovieVote(request);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
        }


        [Fact]
        public async void AddMovieVoteTest()
        {
            long userId = 1;
            long movieId = 1;
            MovieVoteRequest request = new()
            {
                UserId = userId,
                MovieId = movieId,
            };

            _ = _moviesVoteBusiness.Setup(m => m.HasVoted(request.MovieId)).Returns(true);

            MovieVoteController controller = new(_moviesVoteBusiness.Object);
            IActionResult result = await controller.AddMovieVote(request);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(403, okResult?.StatusCode);
        }


        [Fact]
        public async void AddMovieAlreadyVotedTest()
        {
            long userId = 1;
            long movieId = 1;
            MovieVoteRequest request = new()
            {
                UserId = userId,
                MovieId = movieId,
            };

            _ = _moviesVoteBusiness.Setup(m => m.HasVoted(request.MovieId)).Returns(false);
            _ = _moviesVoteBusiness.Setup(m => m.AddVote(request.UserId, request.MovieId)).ReturnsAsync(value: null);

            MovieVoteController controller = new(_moviesVoteBusiness.Object);
            IActionResult result = await controller.AddMovieVote(request);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(400, okResult?.StatusCode);
        }
    }
}
