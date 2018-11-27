namespace Eventures.Controllers
{
    using System.Security.Claims;
    using Eventures.Services;
    using Eventures.ViewModels.Events;
    using Eventures.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;

        public OrdersController(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [HttpPost]
        public IActionResult Create(CreateOrderViewModel model)
        {
            model.CustomerId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (this.ModelState.IsValid)
            {
                this.orderService.CreateOrder(model);
                return this.Redirect("/events/my");
            }

            return this.Redirect("/events/all");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult All()
        {
            var orders = this.orderService.GetAllOrders();
            var model = new AllOrdersViewModel()
            {
                Orders = orders
            };
            return this.View(model);
        }
    }
}
