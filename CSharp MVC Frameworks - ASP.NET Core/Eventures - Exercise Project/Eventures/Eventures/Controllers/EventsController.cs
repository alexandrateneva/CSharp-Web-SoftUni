namespace Eventures.Controllers
{
    using System;
    using Eventures.Filters;
    using Eventures.Loggers;
    using Eventures.Services;
    using Eventures.ViewModels.Events;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public class EventsController : Controller
    {
        private readonly IEventService eventService;
        private readonly ILoggerFactory loggerFactory;

        public EventsController(IEventService eventService, ILoggerFactory loggerFactory)
        {
            this.eventService = eventService;
            this.loggerFactory = loggerFactory;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new CreateEventViewModel();
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [TypeFilter(typeof(LogEventCreationActionFilter))]
        public IActionResult Create(CreateEventViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                this.eventService.CreateEvent(model);

                this.loggerFactory.AddColoredConsoleLogger(c =>
                {
                    c.LogLevel = LogLevel.Information;
                    c.Color = ConsoleColor.Blue;
                    c.Message = $"Event created: {model.Name}";
                });

                this.ViewData["Model"] = model;

                return this.Redirect("/");
            }

            return this.View(model);
        }

        [Authorize]
        public IActionResult All()
        {
            var events = this.eventService.GetAllEvents();
            var model = new AllEventsViewModel()
            {
                Events = events
            };
            return this.View(model);
        }
    }
}
