using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eventures.Services
{
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.ViewModels.Events;

    public class EventService : IEventService
    {
        private readonly ApplicationDbContext context;
        private readonly IOrderService orderService;

        public EventService(ApplicationDbContext context, IOrderService orderService)
        {
            this.context = context;
            this.orderService = orderService;
        }

        public Event CreateEvent(CreateEventViewModel model)
        {
            var @event = new Event()
            {
                Name = model.Name,
                Place = model.Place,
                Start = (DateTime)model.Start,
                End = (DateTime)model.End,
                TotalTickets = model.TotalTickets,
                PricePerTicket = model.PricePerTicket,
            };

            this.context.Events.Add(@event);
            this.context.SaveChanges();

            return @event;
        }

        public IList<BaseEventViewModel> GetAllEvents()
        {
            var events = this.context.Events.Select(e => new BaseEventViewModel()
            {
                Id = e.Id,
                Name = e.Name,
                Place = e.Place,
                Start = e.Start,
                End = e.End
            }).ToList();

            return events;
        }

        public IList<MyEventViewModel> GetCurrentUserEvents(string userId)
        {
            var events = this.context.Events
                .Select(e => new MyEventViewModel()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Place = e.Place,
                    Start = e.Start,
                    End = e.End
                }).ToList();

            foreach (var @event in events)
            {
                var currentId = @event.Id;
                @event.TicketsCount = this.orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser(currentId, userId);
            }

            return events.FindAll(e => e.TicketsCount > 0);
        }
    }
}
