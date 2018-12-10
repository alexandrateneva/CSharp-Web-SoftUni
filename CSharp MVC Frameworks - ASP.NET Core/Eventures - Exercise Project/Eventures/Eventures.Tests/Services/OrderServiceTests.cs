namespace Eventures.Tests.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Eventures.AutoMapper;
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.Services;
    using Eventures.ViewModels.Events;
    using Eventures.ViewModels.Orders;
    using global::AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class OrderServiceTests
    {
        private readonly IMapper mapper;

        public OrderServiceTests()
        {
            //Auto mapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingConfiguration>();
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public void CheckIfOneOrderIsCreatedCorrectly()
        {
            var model = new CreateOrderViewModel()
            {
                CustomerId = "1",
                EventId = "1",
                TicketsCount = 100
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new OrderService(context, this.mapper);
                service.CreateOrder(model);
                Assert.Equal(1, context.Orders.Count());

                var order = context.Orders.FirstOrDefault(e => e.CustomerId == "1");
                Assert.NotNull(order);
            }
        }

        [Fact]
        public void CheckIfMoreOrdersAreCreatedCorrectly()
        {
            var model1 = new CreateOrderViewModel()
            {
                CustomerId = "1",
                EventId = "1",
                TicketsCount = 100
            };
            var model2 = new CreateOrderViewModel()
            {
                CustomerId = "2",
                EventId = "5",
                TicketsCount = 100
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new OrderService(context, this.mapper);
                service.CreateOrder(model1);
                service.CreateOrder(model2);
                Assert.Equal(2, context.Orders.Count());

                var order1 = context.Orders.FirstOrDefault(e => e.CustomerId == "1");
                var order2 = context.Orders.FirstOrDefault(e => e.EventId == "5");

                Assert.NotNull(order1);
                Assert.NotNull(order2);
            }
        }

        [Fact]
        public async Task CheckIfGetTotalBoughtTicketsCountByEventIdAndUserIdWithOneEventAndOneOrder()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(new Event()
                {
                    Id = "1",
                    Name = "Test Event"
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 10
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var result = orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser("1", "1");

                Assert.NotNull(result);
                Assert.Equal(10, result);
            }
        }

        [Fact]
        public async Task CheckIfGetTotalBoughtTicketsCountByEventIdAndUserIdWithOneEventAndManyOrders()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(new Event()
                {
                    Id = "1",
                    Name = "Test Event"
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 11
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 14
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "8",
                    EventId = "1",
                    TicketsCount = 8
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var result = orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser("1", "1");

                Assert.NotNull(result);
                Assert.Equal(25, result);
            }
        }

        [Fact]
        public async Task CheckIfGetTotalBoughtTicketsCountByEventIdAndUserIdWithManyEventsAndManyOrders()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(new Event()
                {
                    Id = "1",
                    Name = "Test Event First"
                });
                context.Events.Add(new Event()
                {
                    Id = "2",
                    Name = "Test Event Second"
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "8",
                    EventId = "1",
                    TicketsCount = 14
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 29
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 8
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var firstEventResult = orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser("1", "1");
                var secondEventResult = orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser("2" , "1");

                Assert.NotNull(firstEventResult);
                Assert.Equal(29, firstEventResult);
                Assert.NotNull(secondEventResult);
                Assert.Equal(18, secondEventResult);
            }
        }

        [Fact]
        public async Task CheckIfGetAllOrdersCorrectly()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "8",
                    EventId = "1",
                    TicketsCount = 14
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 29
                });
                await context.SaveChangesAsync();

                var service = new OrderService(context, this.mapper);
                var result = service.GetAllOrders();
                var elementType = result.First().GetType();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
                Assert.Same(typeof(BaseOrderViewModel), elementType);
            }
        }

        [Fact]
        public void CheckIfReturnEmptyCollectionWhenTryToGetAllOrdersWithoutAnyRecordsInDb()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new OrderService(context, this.mapper);
                var result = service.GetAllOrders();

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task CheckIfGetAllOrdersByUserIdCorrectly()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "8",
                    EventId = "1",
                    TicketsCount = 14
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 29
                });
                await context.SaveChangesAsync();

                var service = new OrderService(context, this.mapper);
                var result = service.GetAllOrdersByUserId("1");

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
            }
        }

        [Fact]
        public async Task CheckIfReturnEmptyCollectionWhenTryToGetAllOrdersByInvalidUserId()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "2",
                    EventId = "1",
                    TicketsCount = 14
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "3",
                    EventId = "1",
                    TicketsCount = 29
                });
                await context.SaveChangesAsync();

                var service = new OrderService(context, this.mapper);
                var result = service.GetAllOrdersByUserId("5");

                Assert.NotNull(result);
                Assert.Equal(0, result.Count());
            }
        }

        private static DbContextOptions<ApplicationDbContext> CreateNewContextOptions()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            builder.UseInMemoryDatabase()
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}
