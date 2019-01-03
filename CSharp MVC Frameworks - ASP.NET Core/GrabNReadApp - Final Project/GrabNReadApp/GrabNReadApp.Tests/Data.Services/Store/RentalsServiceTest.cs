using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Store
{
    public class RentalsServiceTest
    {
        public RentalsServiceTest()
        {
            TestInitializer.Initialize();
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Rentals.AddRange(GetRentals());
            db.SaveChanges();

            var repository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(repository);

            var newRental = new Rental()
            {
                BookId = 7,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };

            //Act
            var enteredRental = await rentalsService.Create(newRental);
            var rentalsCount = db.Rentals.Count();

            //Assert
            Assert.Equal(4, rentalsCount);
            Assert.NotNull(enteredRental);
            Assert.Same(newRental, enteredRental);
        }

        [Fact]
        public async void GetRentalByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Rentals.AddRange(GetRentals());
            var newRental = new Rental()
            {
                Id = 17919,
                BookId = 7,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };
            db.Rentals.Add(newRental);
            db.SaveChanges();

            var repository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(repository);
            
            //Act
            var rental = await rentalsService.GetRentalById(17919);
            var notExistingRental = await rentalsService.GetRentalById(-8199);

            //Assert
            Assert.NotNull(rental);
            Assert.Same(newRental, rental);
            Assert.Null(notExistingRental);
        }

        [Fact]
        public void GetAllRentalsByOrderIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.Add(new Book { Id = 1 });
            db.Books.Add(new Book { Id = 5 });
            db.Rentals.AddRange(GetRentals());
            db.SaveChanges();

            var repository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(repository);

            //Act
            var rentals = rentalsService.GetAllOrderedRentalsByOrderId(42).ToList();
            var notExistingOrderId = rentalsService.GetAllOrderedRentalsByOrderId(97);

            //Assert
            Assert.Equal(2, rentals.Count);
            Assert.Empty(notExistingOrderId);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Rentals.AddRange(GetRentals());
            var newRental = new Rental()
            {
                Id = 17919,
                BookId = 7,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(5),
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };
            db.Rentals.Add(newRental);
            db.SaveChanges();

            var repository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(repository);

            //Act
            var isDeletedRealRental = rentalsService.Delete(17919);
            var isDeletedNotExistingRental = rentalsService.Delete(17919);
            var rentalsCount = db.Rentals.Count();

            //Assert
            Assert.True(isDeletedRealRental);
            Assert.False(isDeletedNotExistingRental);
            Assert.Equal(3, rentalsCount);
        }

        private Rental[] GetRentals()
        {
            return new Rental[]
            {
               new Rental()
               {
                   BookId = 5,
                   StartDate = DateTime.UtcNow,
                   EndDate = DateTime.UtcNow.AddDays(5),
                   CustomerId = "HPQ791KHA0SLAJ",
                   OrderId = 42
               },
               new Rental()
               {
                   BookId = 7,
                   StartDate = DateTime.UtcNow,
                   EndDate = DateTime.UtcNow.AddDays(2),
                   CustomerId = "UOW891KHA0UQO8",
                   OrderId = 78
               },
               new Rental()
               {
                   BookId = 1,
                   StartDate = DateTime.UtcNow,
                   EndDate = DateTime.UtcNow.AddDays(7),
                   CustomerId = "AYI91JAKAO910",
                   OrderId = 42
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
