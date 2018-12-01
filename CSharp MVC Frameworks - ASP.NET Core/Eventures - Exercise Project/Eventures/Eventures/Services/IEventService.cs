namespace Eventures.Services
{
    using System.Collections.Generic;
    using Eventures.Models;
    using Eventures.ViewModels.Events;
    using System.Linq;

    public interface IEventService
    {
        Event CreateEvent(CreateEventViewModel model);

        Event GetEventById(string id);

        IEnumerable<BaseEventViewModel> GetAllEvents();

        IEnumerable<MyEventViewModel> GetCurrentUserEvents(string userId);

        Event DecreaseTicketsCount(Event @event, int boughtTicketsCount);
    }
}
