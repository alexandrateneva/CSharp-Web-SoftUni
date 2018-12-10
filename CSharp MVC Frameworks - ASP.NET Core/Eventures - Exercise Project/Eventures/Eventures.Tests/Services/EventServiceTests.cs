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
    using Microsoft.EntityFrameworkCore;
    using global::AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Moq;
    using Xunit;

    public class EventServiceTests
    {
        private readonly IMapper mapper;

        public EventServiceTests()
        {
            //Auto mapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingConfiguration>();
            });
            this.mapper = config.CreateMapper();
        }

        [Fact]
        public void CheckIfOneEventIsCreatedCorrectly()
        {
            var model = new CreateEventViewModel()
            {
                Name = "Test Event",
                Place = "Sofia, Bulgaria",
                PricePerTicket = 150,
                TotalTickets = 200,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(5)
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new EventService(context, null, this.mapper);
                service.CreateEvent(model);
                Assert.Equal(1, context.Events.Count());

                var @event = context.Events.FirstOrDefault(e => e.Name == "Test Event");
                Assert.NotNull(@event);
            }
        }

        [Fact]
        public void CheckIfMoreEventsAreCreatedCorrectly()
        {
            var model = new CreateEventViewModel()
            {
                Name = "Test Event",
                Place = "Sofia, Bulgaria",
                PricePerTicket = 150,
                TotalTickets = 200,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(5)
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new EventService(context, null, this.mapper);
                service.CreateEvent(model);
                service.CreateEvent(model);
                Assert.Equal(2, context.Events.Count());

                var events = context.Events.Where(e => e.Name == "Test Event");
                Assert.Equal(2, events.Count());
            }
        }

        [Fact]
        public async Task CheckIfGetByIdReturnsCorrectEvent()
        {
            var @event = new Event()
            {
                Id = "1",
                Name = "Test Event",
                Place = "Sofia, Bulgaria",
                PricePerTicket = 150,
                TotalTickets = 200,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(5)
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(@event);
                await context.SaveChangesAsync();

                var service = new EventService(context, null, null);
                var result = service.GetEventById("1");
                Assert.Same(@event, result);
                Assert.NotNull(result);
            }
        }

        [Fact]
        public void CheckIfGetByIdReturnsNullWhenEventDoesNotExist()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new EventService(context, null, null);
                var result = service.GetEventById("1");
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task CheckIfDecreaseTicketsCountCorrectly()
        {
            var @event = new Event()
            {
                Name = "Test Event",
                Place = "Sofia, Bulgaria",
                PricePerTicket = 150,
                TotalTickets = 200,
                Start = DateTime.UtcNow,
                End = DateTime.UtcNow.AddDays(5)
            };
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(@event);
                await context.SaveChangesAsync();

                var service = new EventService(context, null, null);
                var result = service.DecreaseTicketsCount(@event, 50);
                @event.TotalTickets -= 50;
                Assert.Same(@event, result);
            }
        }

        [Fact]
        public async Task CheckIfGetAllEventsAsBaseEventViewModelCorrectly()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                context.Events.Add(new Event()
                {
                    Name = "Test Event First",
                    Place = "Sofia, Bulgaria",
                    PricePerTicket = 1000,
                    TotalTickets = 90,
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(1)
                });
                context.Events.Add(new Event()
                {
                    Name = "Test Event Second",
                    Place = "Plovdiv, Bulgaria",
                    PricePerTicket = 500,
                    TotalTickets = 80,
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(1)
                });
                context.Events.Add(new Event()
                {
                    Name = "Test Event Third",
                    Place = "Varna, Bulgaria",
                    PricePerTicket = 300,
                    TotalTickets = 60,
                    Start = DateTime.UtcNow,
                    End = DateTime.UtcNow.AddDays(1)
                });
                await context.SaveChangesAsync();

                var service = new EventService(context, null, this.mapper);
                var result = service.GetAllEvents();
                var elementType = result.First().GetType();

                Assert.NotNull(result);
                Assert.Equal(3, result.Count());
                Assert.Same(typeof(BaseEventViewModel), elementType);
            }
        }

        [Fact]
        public void CheckIfReturnEmptyCollectionWhenTryToGetAllEventsWithoutAnyRecordsInDb()
        {
            using (var context = new ApplicationDbContext(CreateNewContextOptions()))
            {
                var service = new EventService(context, null, this.mapper);
                var result = service.GetAllEvents();

                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
        
        [Fact]
        public async Task CheckIfGetCurrentUserEventsCorrectlyWithOneEventAndOneOrder()
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
                    EventId = "1"
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var eventService = new EventService(context, orderService, this.mapper);
                var result = eventService.GetCurrentUserEvents("1");
                var elementType = result.First().GetType();

                Assert.NotNull(result);
                Assert.Equal(1, result.Count());
                Assert.Same(typeof(MyEventViewModel), elementType);
            }
        }

        [Fact]
        public async Task CheckIfGetCurrentUserEventsCorrectlyWithOneEventAndMoreOrders()
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
                    CustomerId = "6",
                    EventId = "1",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 27
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "1",
                    TicketsCount = 4
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var eventService = new EventService(context, orderService, this.mapper);
                var result = eventService.GetCurrentUserEvents("1");
                var element = result.First();

                Assert.NotNull(result);
                Assert.Equal(1, result.Count());
                Assert.Equal(31, element.TicketsCount);
                Assert.Same(typeof(MyEventViewModel), element.GetType());
            }
        }

        [Fact]
        public async Task CheckIfGetCurrentUserEventsCorrectlyWithMoreEventsAndMoreOrders()
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
                    EventId = "1",
                    TicketsCount = 10
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "1",
                    EventId = "2",
                    TicketsCount = 27
                });
                context.Orders.Add(new Order()
                {
                    CustomerId = "4",
                    EventId = "1",
                    TicketsCount = 4
                });
                await context.SaveChangesAsync();

                var orderService = new OrderService(context, this.mapper);
                var eventService = new EventService(context, orderService, this.mapper);
                var result = eventService.GetCurrentUserEvents("1");
                var firstEvent = result.First(e => e.Id == "1");
                var secondEvent = result.First(e => e.Id == "2");

                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Equal(10, firstEvent.TicketsCount);
                Assert.Equal(27, secondEvent.TicketsCount);
                Assert.Same(typeof(MyEventViewModel), firstEvent.GetType());
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
