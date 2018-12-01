namespace Eventures.Controllers
{
    using System.Security.Claims;
    using Eventures.Services;
    using Eventures.ViewModels.Events;
    using Eventures.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using X.PagedList;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IEventService eventService;

        public OrdersController(IOrderService orderService, IEventService eventService)
        {
            this.orderService = orderService;
            this.eventService = eventService;
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CreateOrderViewModel model)
        {
            var @event = this.eventService.GetEventById(model.EventId);
            if (@event.TotalTickets <= 0 || @event.TotalTickets < model.TicketsCount)
            {
                var errorModel = new MakeOrderErrorViewModel()
                {
                    EventName = @event.Name,
                    TryToBuyTicketsCount = model.TicketsCount,
                    AllAvailableTicketsCount = @event.TotalTickets
                };
                return this.View("MakeOrderError", errorModel);
            }

            model.CustomerId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (this.ModelState.IsValid)
            {
                this.eventService.DecreaseTicketsCount(@event, model.TicketsCount);
                this.orderService.CreateOrder(model);

                return this.Redirect("/events/my");
            }

            return this.Redirect("/events/all");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All(int? page)
        {
            var pageNumber = page ?? 1;
            var orders = this.orderService.GetAllOrders();
            var onePageOfOrders = orders.ToPagedList(pageNumber, 3);
            var model = new AllOrdersViewModel()
            {
                Orders = onePageOfOrders,
                CurrentPage = pageNumber
            };
            return this.View(model);
        }
    }
}
