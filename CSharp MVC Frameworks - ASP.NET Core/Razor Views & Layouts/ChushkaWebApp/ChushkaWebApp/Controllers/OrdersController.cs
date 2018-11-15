namespace ChushkaWebApp.Controllers
{
    using System.Security.Claims;
    using ChushkaWebApp.Services.Contracts;
    using ChushkaWebApp.ViewModels.Orders;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class OrdersController : Controller
    {
        private readonly IOrderService orderService;
        private readonly IProductService productService;

        public OrdersController(IOrderService orderService, IProductService productService)
        {
            this.orderService = orderService;
            this.productService = productService;
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

        [Authorize]
        public IActionResult Create(int id)
        {
            var product = this.productService.GetProductById(id);
            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

            this.orderService.Create(product.Id, userId);
            return this.Redirect("/");
        }
    }
}
