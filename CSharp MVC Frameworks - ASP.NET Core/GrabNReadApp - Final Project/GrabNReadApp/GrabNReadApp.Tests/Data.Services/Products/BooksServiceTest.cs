using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Enums;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Products
{
    public class BooksServiceTest
    {
        public BooksServiceTest()
        {
            TestInitializer.Initialize();
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            var newBook = new Book()
            {
                Title = "The Notebook",
                Author = "Nicholas Sparks",
                Price = 25.50m,
                PricePerDay = 2.50m,
                Description = "Example description for this book...",
                Pages = 278,
                GenreId = 1,
                CoverType = CoverType.SoftCover
            };

            //Act
            var enteredBook = await booksService.Create(newBook);
            var booksCount = db.Books.Count();

            //Assert
            Assert.Equal(4, booksCount);
            Assert.NotNull(enteredBook);
            Assert.Same(newBook, enteredBook);
        }

        [Fact]
        public void GetAllBooksMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            //Act
            var books = booksService.GetAllBooks().ToList();
            var firstBookInDb = db.Books.FirstOrDefault();

            //Assert
            Assert.Equal(3, books.Count);
            Assert.Equal(firstBookInDb?.Id, books.LastOrDefault()?.Id);  //ORDER BY DESC
        }

        [Fact]
        public async void EditMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            var newBook = new Book()
            {
                Title = "The Notebook",
                Author = "Nicholas Sparks",
                Price = 25.50m,
                PricePerDay = 2.50m,
                Description = "Example description for this book...",
                Pages = 278,
                GenreId = 1,
                CoverType = CoverType.SoftCover
            };
            db.Books.Add(newBook);
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            //Act
            newBook.Price = 35.00m;
            newBook.PricePerDay = 3.00m;

            var enteredBook = await booksService.Edit(newBook);
            var booksCount = db.Books.Count();

            //Assert
            Assert.Equal(4, booksCount);
            Assert.NotNull(enteredBook);
            Assert.Same(newBook, enteredBook);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            var newBook = new Book()
            {
                Id = 167282,
                Title = "The Notebook",
                Author = "Nicholas Sparks",
                Price = 25.50m,
                PricePerDay = 2.50m,
                Description = "Example description for this book...",
                Pages = 278,
                GenreId = 1,
                CoverType = CoverType.SoftCover
            };
            db.Books.Add(newBook);
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            //Act
            var isDeletedRealBook = booksService.Delete(167282);
            var isDeletedNotExistingBook = booksService.Delete(167282);
            var booksCount = db.Books.Count();

            //Assert
            Assert.True(isDeletedRealBook);
            Assert.False(isDeletedNotExistingBook);
            Assert.Equal(3, booksCount);
        }

        [Fact]
        public async void GetByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            var newBook = new Book()
            {
                Id = 167282,
                Title = "The Notebook",
                Author = "Nicholas Sparks",
                Price = 25.50m,
                PricePerDay = 2.50m,
                Description = "Example description for this book...",
                Pages = 278,
                GenreId = 1,
                CoverType = CoverType.SoftCover
            };
            db.Books.Add(newBook);
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            //Act
            var book = await booksService.GetBookById(167282);
            var notExistingBook = await booksService.GetBookById(62891);

            //Assert
            Assert.NotNull(book);
            Assert.Null(notExistingBook);
            Assert.Same(newBook, book);
        }

        [Fact]
        public void GetBooksByTitleMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            db.SaveChanges();

            var repository = new DbRepository<Book>(db);
            var booksService = new BooksService(repository);

            //Act
            var oneBookWithThisTitle = booksService.GetBooksByTitle(" THE booK of wHy ").ToList();
            var manyBookWithThisTitle = booksService.GetBooksByTitle(" ThE ").ToList();
            var noBookWithThisTitle = booksService.GetBooksByTitle(" TeSt ").ToList();

            //Assert
            Assert.Single(oneBookWithThisTitle);
            Assert.Equal(2, manyBookWithThisTitle.Count);
            Assert.Empty(noBookWithThisTitle);
        }

        private Book[] GetBooks()
        {
            return new Book[]
            {
               new Book()
               {
                   Title = "The Book Of Why",
                   Author = "Judea Pearl",
                   Price = 25.50m,
                   PricePerDay = 2.50m,
                   Description = "Example description for first book...",
                   Pages = 360,
                   GenreId = 1,
                   CoverType = CoverType.HardCover,
                   ReleaseDate = DateTime.UtcNow
               },
               new Book()
               {
                   Title = "Taking Medicine",
                   Author = "Lyn Miller",
                   Price = 50.00m,
                   PricePerDay = 3.00m,
                   Description = "Example description for second book...",
                   Pages = 520,
                   GenreId = 2,
                   CoverType = CoverType.HardCover,
                   ReleaseDate = DateTime.UtcNow.AddDays(5)
               },
               new Book()
               {
                   Title = "The jungle crew",
                   Author = "Emma Scott",
                   Price = 17.00m,
                   PricePerDay = 1.50m,
                   Description = "Example description for third book...",
                   Pages = 66,
                   GenreId = 3,
                   CoverType = CoverType.SoftCover,
                   ReleaseDate = DateTime.UtcNow.AddDays(3)
               },
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
