namespace Eventures.Services
{
    using System.Collections.Generic;
    using Eventures.Models;
    using Eventures.ViewModels.Events;

    public interface IEventService
    {
        Event CreateEvent(CreateEventViewModel model);

        IList<BaseEventViewModel> GetAllEvents();

        IList<MyEventViewModel> GetCurrentUserEvents(string userId);
    }
}
