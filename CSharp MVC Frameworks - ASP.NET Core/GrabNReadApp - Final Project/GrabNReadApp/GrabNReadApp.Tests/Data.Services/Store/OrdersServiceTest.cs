using System;
using System.Linq;
using AutoMapper;
using GrabNReadApp.Data;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace GrabNReadApp.Tests.Data.Services.Store
{
    public class OrdersServiceTest
    {
        [Fact]
        public void GetOrderByIdWithPurchasesAndRentalsMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            db.Purchases.AddRange(GetPurchases());
            db.Rentals.AddRange(GetRentals());
            db.Orders.AddRange(GetOrders());
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            var order = ordersService.GetOrderByIdWithPurchasesAndRentals(1816);
            var purchasesCount = order.Purchases.Count();
            var rentalsCount = order.Rentals.Count();

            //Assert
            Assert.Equal(1, purchasesCount);
            Assert.Equal(2, rentalsCount);
            Assert.NotNull(order);
        }

        [Fact]
        public void GetCurrentOrderByUserIdWithPurchasesAndRentalsMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Books.AddRange(GetBooks());
            db.Purchases.AddRange(GetPurchases());
            db.Rentals.AddRange(GetRentals());
            db.Orders.AddRange(GetOrders());
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            var notFinishedOrder = ordersService.GetCurrentOrderByUserIdWithPurchasesAndRentals("HPQ791KHA0SLAJ");
            var purchasesCount = notFinishedOrder.Purchases.Count();
            var rentalsCount = notFinishedOrder.Rentals.Count();

            var notExistingOrder = ordersService.GetCurrentOrderByUserIdWithPurchasesAndRentals("PAWNA719AN27AM");

            //Assert
            Assert.NotNull(notFinishedOrder);
            Assert.Equal(2, purchasesCount);
            Assert.Equal(1, rentalsCount);
            Assert.Null(notExistingOrder);
        }

        [Fact]
        public async void CreateMethodHaveToWorkCorrectly()
        {
            var db = GetDatabase();
            db.Orders.AddRange(GetOrders());
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            var newOrder = new Order()
            {
                Id = 4,
                Address = "Sofia, Bulgaria",
                CustomerId = "TPQY1729QH71KA",
                IsFinished = false,
                Phone = "0889674972",
                Delivery = 5.00m,
                RecipientName = "Peter Petrov"
            };

            //Act
            var enteredOrder = await ordersService.Create(newOrder);
            var ordersCount = db.Orders.Count();

            //Assert
            Assert.Equal(4, ordersCount);
            Assert.NotNull(enteredOrder);
            Assert.Same(newOrder, enteredOrder);
        }

        [Fact]
        public async void UpdateMethodHaveToWorkCorrectly()
        {
            var db = GetDatabase();
            db.Orders.AddRange(GetOrders());
            var newOrder = new Order()
            {
                Id = 7182,
                Address = "Sofia, Bulgaria",
                CustomerId = "TPQY1729QH71KA",
                IsFinished = false,
                Phone = "0889674972",
                Delivery = 5.00m,
                RecipientName = "Ivan Ivanov"
            };
            db.Orders.Add(newOrder);
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            newOrder.OrderedOn = DateTime.UtcNow;
            newOrder.IsFinished = true;

            var enteredOrder = await ordersService.Update(newOrder);
            var ordersCount = db.Orders.Count();

            //Assert
            Assert.Equal(4, ordersCount);
            Assert.NotNull(enteredOrder);
            Assert.Same(newOrder, enteredOrder);
        }

        [Fact]
        public void DeleteMethodHaveToWorkCorrectly()
        {
            var db = GetDatabase();
            db.Orders.AddRange(GetOrders());
            var newOrder = new Order()
            {
                Id = 167282,
                Address = "Sofia, Bulgaria",
                CustomerId = "TPQY1729QH71KA",
                IsFinished = false,
                Phone = "0889674972",
                Delivery = 5.00m,
                RecipientName = "Valentin Ivanov"
            };
            db.Orders.Add(newOrder);
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            var isDeletedRealOrder = ordersService.Delete(167282);
            var isDeletedNotExistingOrder = ordersService.Delete(167282);
            var ordersCount = db.Orders.Count();

            //Assert
            Assert.True(isDeletedRealOrder);
            Assert.False(isDeletedNotExistingOrder);
            Assert.Equal(3, ordersCount);
        }

       [Fact]
        public void GetAllFinishedOrdersMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Orders.AddRange(GetOrders());
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            var allFinishedOrder = ordersService.GetAllFinishedOrders();

            //Assert
            Assert.Equal(2, allFinishedOrder.Count());
        }

        [Fact]
        public async void EmptyCurrentUserOrderMethodHaveToWorkCorrectly()
        {
            //Arrange
            var db = GetDatabase();
            db.Orders.AddRange(GetOrders());
            var order = new Order()
            {
                Id = 8190,
                Address = "Sofia, Bulgaria",
                CustomerId = "TPQY1729QH71KA",
                IsFinished = false,
                Phone = "0889674972",
                Delivery = 5.00m,
                RecipientName = "Ivan Ivanov"
            };
            db.Orders.Add(order);
            db.SaveChanges();

            var purchaseRepository = new DbRepository<Purchase>(db);
            var purchasesService = new PurchasesService(purchaseRepository);

            var rentalsRepository = new DbRepository<Rental>(db);
            var rentalsService = new RentalsService(rentalsRepository);

            var ordersRepository = new DbRepository<Order>(db);
            var ordersService = new OrdersService(ordersRepository, purchasesService, rentalsService);

            //Act
            await ordersService.EmptyCurrentUserOrder(order.Id, order);

            var oldOrder = db.Orders.FirstOrDefault(o => o.Id == order.Id);
            var newOrder = db.Orders.FirstOrDefault(o => o.CustomerId == order.CustomerId
                                                         && o.IsFinished == false
                                                         && o.Id != oldOrder.Id);
            //Assert
            Assert.NotNull(oldOrder);
            Assert.NotSame(newOrder, oldOrder);
            Assert.Equal(DateTime.UtcNow.Date, oldOrder.OrderedOn.Date);
            Assert.True(oldOrder.IsFinished);
            Assert.NotNull(newOrder);
        }
      
        private Book[] GetBooks()
        {
            return new Book[]
            {
              new Book()
              {
                  Id = 1
              },
              new Book()
              {
                  Id = 7
              },
              new Book()
              {
                  Id = 9
              },
            };
        }

        private Order[] GetOrders()
        {
            return new Order[]
            {
               new Order()
               {
                   Id = 1816,
                   Address = "Sofia, Bulgaria",
                   CustomerId = "HPQ791KHA0SLAJ",
                   IsFinished = true,
                   Phone = "0889674972",
                   Delivery = 5.00m,
                   RecipientName = "Ivan Georgiev"
               },
               new Order()
               {
                   Id = 2781,
                   Address = "Sofia, Bulgaria",
                   CustomerId = "HPQ791KHA0SLAJ",
                   IsFinished = false,
                   Phone = "0889674972",
                   Delivery = 5.00m,
                   RecipientName = "Georgi Angelov"
               },
               new Order()
               {
                   Id = 3017,
                   Address = "Burgas, Bulgaria",
                   CustomerId = "UOW891KHA0UQO8",
                   IsFinished = true,
                   Phone = "0889674972",
                   Delivery = 4.00m,
                   RecipientName = "Peter Ivanov"
               },
            };
        }

        private Purchase[] GetPurchases()
        {
            return new Purchase[]
            {
                new Purchase()
                {
                    BookId = 1,
                    BookCount = 2,
                    OrderId = 2781
                },
                new Purchase()
                {
                    BookId = 9,
                    BookCount = 1,
                    OrderId = 1816
                },
                new Purchase()
                {
                    BookId = 7,
                    BookCount = 5,
                    OrderId = 2781
                },
            };
        }

        private Rental[] GetRentals()
        {
            return new Rental[]
            {
                new Rental()
                {
                    BookId = 9,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(5),
                    OrderId = 1816
                },
                new Rental()
                {
                    BookId = 7,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(2),
                    OrderId = 2781
                },
                new Rental()
                {
                    BookId = 1,
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(7),
                    OrderId = 1816
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
