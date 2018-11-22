namespace Eventures.Services
{
    using System;
    using System.Collections.Generic;
    using Eventures.Models;
    using Eventures.ViewModels.Events;

    public interface IEventService
    {
        Event CreateEvent(CreateEventViewModel model);

        IList<BaseEventViewModel> GetAllEvents();
    }
}
