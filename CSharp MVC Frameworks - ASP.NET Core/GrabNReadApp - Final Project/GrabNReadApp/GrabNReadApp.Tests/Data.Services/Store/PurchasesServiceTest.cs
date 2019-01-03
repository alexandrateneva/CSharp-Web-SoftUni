using System;
using System.Linq;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Enums;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Store
{
    public class PurchasesServiceTest
    {
        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Purchases.AddRange(GetPurchases());
            db.SaveChanges();

            var repository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(repository);

            var newPurchase = new Purchase()
            {
                BookId = 7,
                BookCount = 2,
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };

            //Act
            var enteredPurchase = await purchasesService.Create(newPurchase);
            var purchasesCount = db.Purchases.Count();

            //Assert
            Assert.Equal(4, purchasesCount);
            Assert.NotNull(enteredPurchase);
            Assert.Same(newPurchase, enteredPurchase);
        }

        [Fact]
        public async void GetPurchaseByIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Purchases.AddRange(GetPurchases());
            var newPurchase = new Purchase()
            {
                Id = 17919,
                BookId = 7,
                BookCount = 2,
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };
            db.Purchases.Add(newPurchase);
            db.SaveChanges();

            var repository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(repository);
            
            //Act
            var purchase = await purchasesService.GetPurchaseById(17919);
            var notExistingPurchase = await purchasesService.GetPurchaseById(-8199);

            //Assert
            Assert.NotNull(purchase);
            Assert.Same(newPurchase, purchase);
            Assert.Null(notExistingPurchase);
        }

        [Fact]
        public void GetAllPurchasesByOrderIdMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.Add(new Book {Id = 1});
            db.Books.Add(new Book { Id = 7 });
            db.Purchases.AddRange(GetPurchases());
            db.SaveChanges();

            var repository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(repository);

            //Act
            var purchases = purchasesService.GetAllOrderedPurchasesByOrderId(38).ToList();
            var notExistingOrderId = purchasesService.GetAllOrderedPurchasesByOrderId(97);

            //Assert
            Assert.Equal(2,purchases.Count);
            Assert.Empty(notExistingOrderId);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Purchases.AddRange(GetPurchases());
            var newPurchase = new Purchase()
            {
                Id = 167282,
                BookId = 7,
                BookCount = 2,
                CustomerId = "0X7PQJ81HKA9JQO",
                OrderId = 38
            };
            db.Purchases.Add(newPurchase);
            db.SaveChanges();

            var repository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(repository);

            //Act
            var isDeletedRealPurchase = purchasesService.Delete(167282);
            var isDeletedNotExistingPurchase = purchasesService.Delete(167282);
            var purchasesCount = db.Purchases.Count();

            //Assert
            Assert.True(isDeletedRealPurchase);
            Assert.False(isDeletedNotExistingPurchase);
            Assert.Equal(3, purchasesCount);
        }

        private Purchase[] GetPurchases()
        {
            return new Purchase[]
            {
               new Purchase()
               {
                   BookId = 1,
                   BookCount = 2,
                   CustomerId = "HPQ791KHA0SLAJ",
                   OrderId = 38
               },
               new Purchase()
               {
                   BookId = 9,
                   BookCount = 1,
                   CustomerId = "UOW891KHA0UQO8",
                   OrderId = 42
               },
               new Purchase()
               {
                   BookId = 7,
                   BookCount = 5,
                   CustomerId = "AYI91JAKAO910",
                   OrderId = 38
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
