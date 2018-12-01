namespace Eventures.Services
{
    using Eventures.Data;
    using Eventures.Models;
    using Eventures.ViewModels.Events;
    using global::AutoMapper;
    using System.Collections.Generic;
    using System.Linq;

    public class EventService : IEventService
    {
        private readonly ApplicationDbContext context;
        private readonly IOrderService orderService;
        private readonly IMapper mapper;

        public EventService(ApplicationDbContext context, IOrderService orderService, IMapper mapper)
        {
            this.context = context;
            this.orderService = orderService;
            this.mapper = mapper;
        }

        public Event CreateEvent(CreateEventViewModel model)
        {
            var @event = this.mapper.Map<Event>(model);

            this.context.Events.Add(@event);
            this.context.SaveChanges();

            return @event;
        }

        public Event GetEventById(string id)
        {
            var @event = this.context.Events.FirstOrDefault(e => e.Id == id);
            return @event;
        }

        public Event DecreaseTicketsCount(Event @event, int boughtTicketsCount)
        {
            var finalTicketsCount = @event.TotalTickets - boughtTicketsCount;
            @event.TotalTickets = finalTicketsCount;

            this.context.Events.Update(@event);
            this.context.SaveChanges();

            return @event;
        }

        public IEnumerable<BaseEventViewModel> GetAllEvents()
        {
            var events = this.context.Events
                .Where(x => x.TotalTickets > 0)
                .Select(e => this.mapper.Map<BaseEventViewModel>(e));

            return events;
        }

        public IEnumerable<MyEventViewModel> GetCurrentUserEvents(string userId)
        {
            var eventsIdsOfUserOrders = this.orderService.GetAllOrdersByUserId(userId).Select(o => o.EventId).ToList();

            var events = this.context.Events
                .Where(x => eventsIdsOfUserOrders.Contains(x.Id))
                .Select(e => this.mapper.Map<MyEventViewModel>(e))
                .ToList();

            foreach (var @event in events)
            {
                var currentId = @event.Id;
                @event.TicketsCount = this.orderService.GetTotalBoughtTicketsCountByEventIdFromCurrentUser(currentId, userId);
            }

            return events;
        }
    }
}
