using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Products
{
    public class GenresServiceTest
    {
        public GenresServiceTest()
        {
            TestInitializer.Initialize();
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Genre.AddRange(GetGenres());
            db.SaveChanges();

            var repository = new DbRepository<Genre>(db);
            var genresService = new GenresService(repository);

            var newGenre = new Genre()
            {
                Name = "Mystery"
            };

            //Act
            var enteredGenre = await genresService.Create(newGenre);
            var genresCount = db.Genre.Count();

            //Assert
            Assert.Equal(4, genresCount);
            Assert.NotNull(enteredGenre);
            Assert.Same(newGenre, enteredGenre);
        }

        [Fact]
        public async void EditMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Genre.AddRange(GetGenres());

            var newGenre = new Genre()
            {
                Name = "Mystery"
            };
            db.Genre.Add(newGenre);
            db.SaveChanges();

            var repository = new DbRepository<Genre>(db);
            var genresService = new GenresService(repository);
            
            //Act
            newGenre.Name = "Mystery & Thriller";
            var changedGenre = await genresService.Edit(newGenre);
            var genresCount = db.Genre.Count();

            //Assert
            Assert.Equal(4, genresCount);
            Assert.NotNull(changedGenre);
            Assert.Same(newGenre, changedGenre);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Genre.AddRange(GetGenres());

            var newGenre = new Genre()
            {
                Id = 26818,
                Name = "Mystery"
            };
            db.Genre.Add(newGenre);
            db.SaveChanges();

            var repository = new DbRepository<Genre>(db);
            var genresService = new GenresService(repository);

            //Act
            var isDeletedRealGenre = genresService.Delete(26818);
            var isDeletedNotExistingGenre = genresService.Delete(26818);
            var genresCount = db.Genre.Count();

            //Assert
            Assert.Equal(3, genresCount);
            Assert.True(isDeletedRealGenre);
            Assert.False(isDeletedNotExistingGenre);
        }

        [Fact]
        public async void GetGenreByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Genre.AddRange(GetGenres());

            var newGenre = new Genre()
            {
                Id = 82901,
                Name = "Mystery"
            };
            db.Genre.Add(newGenre);
            db.SaveChanges();

            var repository = new DbRepository<Genre>(db);
            var genresService = new GenresService(repository);

            //Act
            var genre = await genresService.GetGenreById(82901);
            var notExistingGenre = await genresService.GetGenreById(71939);

            //Assert
            Assert.Null(notExistingGenre);
            Assert.NotNull(genre);
            Assert.Same(newGenre, genre);
        }

        [Fact]
        public void GetAllGenresMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Genre.AddRange(GetGenres());
            db.SaveChanges();

            var repository = new DbRepository<Genre>(db);
            var genresService = new GenresService(repository);

            //Act
            var genres = genresService.GetAllGenres().ToList().Count;

            //Assert
            Assert.Equal(3, genres);
        }

        private Genre[] GetGenres()
        {
            return new Genre[]
            {
                new Genre()
                {
                    Name = "Romance"
                },
                new Genre()
                {
                    Name = "Science"
                },
                new Genre()
                {
                    Name = "Fantasy"
                }
            };
        }

        private GrabNReadAppContext GetDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder<GrabNReadAppContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new GrabNReadAppContext(dbOptions);
        }
    }
}
