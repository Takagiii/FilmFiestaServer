using AutoMapper;
using FilmFiesta.Business;
using FilmFiesta.DataAccess;
using FilmFiesta.DataAccess.Interfaces;
using Moq;

namespace FilmFiestaTests.Genre
{
    public class GenreBusinessTests
    {
        public required Mock<IGenresRepository> _genresRepository = new();
        private readonly MapperConfiguration mapperConfig = new(cfg =>
        {
            cfg.AddProfile<AutomapperProfiles>();
        });

        [Fact]
        public void GetAllTest()
        {
            List<FilmFiesta.Dbo.Genre> genres = [];
            _ = _genresRepository.Setup(m => m.GetAll())
                             .Callback(() => genres = [])
                             .Returns(() => genres);

            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            IEnumerable<FilmFiesta.Dbo.Genre> result = genresBusiness.GetAll();
            Assert.Equal(genres, result);
        }

        [Fact]
        public void GetAllExistEmptyTest()
        {
            List<string> genres = [];

            IMapper mapper = mapperConfig.CreateMapper();
            GenresBusiness genresBusiness = new(_genresRepository.Object, mapper);
            bool result = genresBusiness.GenreAllExist(genres);
            Assert.True(result);
        }


        [Fact]
        public void GenreAllExistTest()
        {
            List<FilmFiesta.Dbo.Genre> existingGenres =
            [
                new FilmFiesta.Dbo.Genre { Name = "Action" },
                new FilmFiesta.Dbo.Genre { Name = "Drama" },
            ];

            _ = _genresRepository.Setup(m => m.GetAll()).Returns(existingGenres);

            List<string> genres = ["Action", "Drama"];

            IMapper mapper = mapperConfig.CreateMapper();
            GenresBusiness genresBusiness = new(_genresRepository.Object, mapper);
            bool result = genresBusiness.GenreAllExist(genres);
            Assert.True(result);
        }

        [Fact]
        public void GenreAllExistFalseTest()
        {
            List<FilmFiesta.Dbo.Genre> existingGenres =
            [
                new FilmFiesta.Dbo.Genre { Name = "Action" },
                new FilmFiesta.Dbo.Genre { Name = "Drama" },
            ];

            _ = _genresRepository.Setup(m => m.GetAll()).Returns(existingGenres);

            List<string> genres = ["Action", "Drama", "Comedy"];

            IMapper mapper = mapperConfig.CreateMapper();
            GenresBusiness genresBusiness = new(_genresRepository.Object, mapper);
            bool result = genresBusiness.GenreAllExist(genres);
            Assert.False(result);
        }

        [Fact]
        public void GenreExistsTest()
        {
            string genre = "Action";

            _ = _genresRepository.Setup(m => m.GetGenre(genre)).Returns(new FilmFiesta.Dbo.Genre() { Name = "Action" });

            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            bool result = genresBusiness.GenreExists(genre);
            Assert.True(result);
        }

        [Fact]
        public void GenreExistsFalseTest()
        {
            string genre = "Action";

            _ = _genresRepository.Setup(m => m.GetGenre(genre)).Returns(value: null);

            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            bool result = genresBusiness.GenreExists(genre);
            Assert.False(result);
        }

        [Fact]
        public async Task GenreAddTest()
        {
            string genreNameToAdd = "Action";
            FilmFiesta.Dbo.Genre genreToAdd = new() { Name = genreNameToAdd };

            FilmFiesta.Dbo.Genre insertedGenre = new() { Name = "Action" };

            _ = _genresRepository.Setup(m => m.Insert(genreToAdd))
                .ReturnsAsync(new FilmFiesta.Dbo.Genre() { Name = "Action" })
                .Callback(() =>
                {
                    insertedGenre = new FilmFiesta.Dbo.Genre() { Name = "Action" };
                });

            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            FilmFiesta.Dbo.Genre result = await genresBusiness.GenreAdd(genreNameToAdd);

            Assert.Equal(insertedGenre, result);
        }

        [Fact]
        public async Task GenreRemove()
        {
            long genreId = 1;
            _ = _genresRepository.Setup(m => m.Delete(genreId)).ReturnsAsync(true);
            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            bool result = await genresBusiness.GenreRemove(genreId);

            Assert.True(result);
        }

        [Fact]
        public async Task GenreUpdateTest()
        {
            long genreId = 1;
            string genreToUpdateName = "Drama";
            FilmFiesta.Dbo.Genre genreToUpdate = new() { Id = genreId, Name = genreToUpdateName };
            FilmFiesta.Dbo.Genre updatedGenre = new() { Id = genreId, Name = genreToUpdateName };

            _ = _genresRepository.Setup(m => m.Update(genreToUpdate))
                 .ReturnsAsync(updatedGenre);

            GenresBusiness genresBusiness = new(_genresRepository.Object, null);
            FilmFiesta.Dbo.Genre result = await genresBusiness.GenreUpdate(genreId, genreToUpdateName);

            Assert.Equal(updatedGenre, result);
        }
    }
}
