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
            return this.View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [TypeFilter(typeof(LogEventCreationActionFilter))]
        public IActionResult Create(CreateEventViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Name", "Please provide valid name of event with length of 3 or more characters.");
                return this.View();
            }

            if (string.IsNullOrWhiteSpace(model.Place) || model.Place.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Place", "Please provide valid place of event with length of 3 or more characters.");
                return this.View();
            }

            if (model.Start == null || !DateTime.TryParse(model.Start, out var startDate))
            {
                this.ModelState.AddModelError("Start", "Please provide valid start date of event.");
                return this.View();
            }

            if (model.End == null || !DateTime.TryParse(model.End, out var endDate))
            {
                this.ModelState.AddModelError("Start", "Please provide valid end date of event.");
                return this.View();
            }

            if (model.TotalTickets == null || model.TotalTickets <= 0)
            {
                this.ModelState.AddModelError("TotalTickets", "Total tickets count can not be zero or negative number.");
                return this.View();
            }

            if (model.PricePerTicket == null || model.PricePerTicket <= 0)
            {
                this.ModelState.AddModelError("PricePerTicket", "Price per ticket can not be zero or negative number.");
                return this.View();
            }

            this.eventService.CreateEvent(model, startDate, endDate);

            this.loggerFactory.AddColoredConsoleLogger(c =>
            {
                c.LogLevel = LogLevel.Information;
                c.Color = ConsoleColor.Blue;
                c.Message = $"Event created: {model.Name}";
            });

            this.ViewData["Model"] = model;

            return this.Redirect("/");
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
