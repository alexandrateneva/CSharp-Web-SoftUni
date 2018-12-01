namespace Eventures.Controllers
{
    using System;
    using System.Security.Claims;
    using Eventures.Filters;
    using Eventures.Loggers;
    using Eventures.Services;
    using Eventures.ViewModels.Events;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using X.PagedList;

    public class EventsController : Controller
    {
        private readonly IEventService eventService;
        private readonly IOrderService orderService;
        private readonly ILoggerFactory loggerFactory;

        public EventsController(IEventService eventService, IOrderService orderService, ILoggerFactory loggerFactory)
        {
            this.eventService = eventService;
            this.orderService = orderService;
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
        public IActionResult All(int? page)
        {
            var pageNumber = page ?? 1;
            var events = this.eventService.GetAllEvents();
            var onePageOfEvents = events.ToPagedList(pageNumber, 3);

            var model = new AllEventsViewModel()
            {
                Events = onePageOfEvents,
                CurrentPage = pageNumber
            };
            return this.View(model);
        }

        [Authorize]
        public IActionResult My(int? page)
        {
            var pageNumber = page ?? 1;
            var customerId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var events = this.eventService.GetCurrentUserEvents(customerId);
            var onePageOfEvents = events.ToPagedList(pageNumber, 3);
            var model = new AllMyEventsViewModel()
            {
                Events = onePageOfEvents,
                CurrentPage = pageNumber
            };
            return this.View(model);
        }
    }
}
