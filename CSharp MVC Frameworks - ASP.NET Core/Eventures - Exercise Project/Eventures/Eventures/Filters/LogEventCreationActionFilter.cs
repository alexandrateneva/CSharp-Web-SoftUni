namespace Eventures.Filters
{
    using System;
    using System.Linq;
    using Eventures.Loggers;
    using Eventures.Models;
    using Eventures.ViewModels.Events;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Logging;

    public class LogEventCreationActionFilter : ActionFilterAttribute
    {
        private readonly ILoggerFactory loggerFactory;

        public LogEventCreationActionFilter(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory;
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var username = context.HttpContext.User.Identity.Name;
            var model = ((Controller)context.Controller).ViewData.Values.FirstOrDefault();
            if (model != null)
            {
                var eventModel = (CreateEventViewModel)model;
                this.loggerFactory.AddColoredConsoleLogger(c =>
                {
                    c.LogLevel = LogLevel.Information;
                    c.Color = ConsoleColor.Magenta;
                    c.Message =
                        $"[{DateTime.UtcNow}] Administrator {username} create event {eventModel.Name} ({eventModel.Start} / {eventModel.End})";
                });
            }
            base.OnActionExecuted(context);
        }
    }
}
