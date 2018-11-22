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

        public EventService(ApplicationDbContext context)
        {
            this.context = context;
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
                Name = e.Name,
                Place = e.Place,
                Start = e.Start,
                End = e.End
            }).ToList();

            return events;
        }
    }
}
