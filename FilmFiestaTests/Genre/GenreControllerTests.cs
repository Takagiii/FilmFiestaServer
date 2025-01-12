using FilmFiesta.Business.Interfaces;
using FilmFiesta.Controllers.Genre;
using FilmFiesta.Requests.Genres;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FilmFiestaTests.Genre
{
    public class GenreControllerTests
    {
        public required Mock<IGenresBusiness> _mockGenresBusiness = new();

        [Fact]
        public async void AddGenreTest()
        {
            string genreName = "Action";
            _ = _mockGenresBusiness.Setup(m => m.GenreAdd(genreName))
                              .ReturnsAsync(new FilmFiesta.Dbo.Genre { Name = genreName });

            GenreAddController controller = new(_mockGenresBusiness.Object);
            GenreAddRequest request = new() { genreName = "Action" };

            IActionResult result = await controller.AddGenre(request);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);

            FilmFiesta.Dbo.Genre? genre = okResult?.Value as FilmFiesta.Dbo.Genre;
            _ = Assert.IsType<FilmFiesta.Dbo.Genre>(genre);
            Assert.NotNull(genre);
            Assert.Equal("Action", genre.Name);
        }

        [Fact]
        public void ListGenreTest()
        {
            _ = _mockGenresBusiness.Setup(m => m.GetAll())
                              .Returns([new FilmFiesta.Dbo.Genre() { Name = "Action" }]);

            GenreListController controller = new(_mockGenresBusiness.Object);

            IActionResult result = controller.GetAllGenres();
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);

            IEnumerable<FilmFiesta.Dbo.Genre>? genre = okResult?.Value as IEnumerable<FilmFiesta.Dbo.Genre>;
            _ = Assert.IsAssignableFrom<IEnumerable<FilmFiesta.Dbo.Genre>>(genre);
            Assert.NotNull(genre);
            Assert.True(genre.Count() == 1);
        }

        [Fact]
        public void ListGenreEmptyTest()
        {
            _ = _mockGenresBusiness.Setup(m => m.GetAll())
                              .Returns([]);

            GenreListController controller = new(_mockGenresBusiness.Object);

            IActionResult result = controller.GetAllGenres();
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(404, okResult?.StatusCode);

            string? genreError = okResult?.Value as string;
            _ = Assert.IsType<string>(genreError);
        }

        [Fact]
        public async void RemoveGenreTest()
        {
            long genreId = 1;
            _ = _mockGenresBusiness.Setup(m => m.GenreRemove(genreId))
                              .ReturnsAsync(true);

            GenreRemoveController controller = new(_mockGenresBusiness.Object);

            IActionResult result = await controller.RemoveGenre(genreId);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);
            _ = Assert.IsType<bool>(okResult?.Value);
            Assert.Equal(true, okResult?.Value);
        }

        [Fact]
        public async void RemoveGenreFalseTest()
        {
            long genreId = 1;
            _ = _mockGenresBusiness.Setup(m => m.GenreRemove(genreId))
                              .ReturnsAsync(false);

            GenreRemoveController controller = new(_mockGenresBusiness.Object);

            IActionResult result = await controller.RemoveGenre(genreId);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(400, okResult?.StatusCode);
            _ = Assert.IsType<string>(okResult?.Value);
        }

        [Fact]
        public async void UpdateGenreTest()
        {
            GenreUpdateRequest genreUpdateRequest = new()
            {
                genreId = 1,
                genreName = "Action"
            };
            _ = _mockGenresBusiness
                .Setup(m => m.GenreUpdate(genreUpdateRequest.genreId, genreUpdateRequest.genreName))
                .ReturnsAsync(new FilmFiesta.Dbo.Genre() { Id = 1, Name = "Action" });

            GenreUpdateController controller = new(_mockGenresBusiness.Object);

            IActionResult result = await controller.UpdateGenre(genreUpdateRequest);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(200, okResult?.StatusCode);

            FilmFiesta.Dbo.Genre? genre = okResult?.Value as FilmFiesta.Dbo.Genre;
            _ = Assert.IsType<FilmFiesta.Dbo.Genre>(genre);
            Assert.Equal("Action", genre.Name);
        }

        [Fact]
        public async void UpdateGenreNullTest()
        {
            GenreUpdateRequest genreUpdateRequest = new()
            {
                genreId = 1,
                genreName = "Action"
            };
            _ = _mockGenresBusiness
                .Setup(m => m.GenreUpdate(genreUpdateRequest.genreId, genreUpdateRequest.genreName))
                .ReturnsAsync(value: null);

            GenreUpdateController controller = new(_mockGenresBusiness.Object);

            IActionResult result = await controller.UpdateGenre(genreUpdateRequest);
            ObjectResult? okResult = result as ObjectResult;
            Assert.Equal(500, okResult?.StatusCode);
            _ = Assert.IsType<string>(okResult?.Value);
        }
    }
}
