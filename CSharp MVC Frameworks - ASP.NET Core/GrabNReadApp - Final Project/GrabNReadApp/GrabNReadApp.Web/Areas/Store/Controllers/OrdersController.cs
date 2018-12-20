using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using GrabNReadApp.Web.Areas.Store.Models.Orders;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GrabNReadApp.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class OrdersController : Controller
    {
        private readonly IMapper mapper;
        private readonly IOrdersService ordersService;
        private UserManager<GrabNReadAppUser> userManager;

        public OrdersController(IMapper mapper, IOrdersService ordersService, UserManager<GrabNReadAppUser> userManager)
        {
            this.mapper = mapper;
            this.ordersService = ordersService;
            this.userManager = userManager;
        }

        // GET: Store/Orders/Cart
        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var order = this.ordersService.GetCurrentOrderByUserIdWithPurchasesAndRentals(user.Id);

            var model = mapper.Map<OrderBaseViewModel>(order);
            return View(model);
        }

        // GET: Store/Orders/Order
        [Authorize]
        [Route("Store/Order")]
        public async Task<IActionResult> Order()
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            var order = this.ordersService.GetCurrentOrderByUserIdWithPurchasesAndRentals(user.Id);

            var model = mapper.Map<OrderDetailsViewModel>(order);
            return View(model);
        }

        // POST: Store/Orders/Order
        [HttpPost]
        [Authorize]
        [Route("Store/Order")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Order(OrderDetailsViewModel model)
        {
            var user = await userManager.GetUserAsync(HttpContext.User);
            model.CustomerId = user.Id;
            if (ModelState.IsValid)
            {
                var order = this.ordersService.GetOrderById(model.Id);

                if (model.Address != order.Address || model.Phone != order.Phone ||
                    model.RecipientName != order.RecipientName)
                {
                    var updatedOrder = mapper.Map<Order>(model);
                    await this.ordersService.Update(updatedOrder);
                }

                await this.ordersService.EmptyCurrentOrder(order.Id);

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess("Thank you!", "Your order was successful.");
            }

            return this.View(model);
        }
    }
}