using System.Linq;
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
                var order = mapper.Map<Order>(model);

                await this.ordersService.EmptyCurrentUserOrder(model.Id, order);

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess("Thank you!", "Your order was successful.");
            }

            return this.View(model);
        }

        // GET: Store/Orders/All
        [Authorize]
        public IActionResult All()
        {
            var orders = this.ordersService.GetAllFinishedOrders().ToList();
            var model = new AllOrdersViewModel()
            {
                Orders = orders
            };
            return View(model);
        }

        // GET: Store/Orders/Details/5
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var order = this.ordersService.GetOrderByIdWithPurchasesAndRentals(id);
            var model = mapper.Map<OrderDetailsViewModel>(order);
            return View(model);
        }

        // GET: Store/Orders/Details/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var isDeleted = this.ordersService.Delete(id);
            if (!isDeleted)
            {
                var error = new Error() { Message = "Delete failed." };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Orders").WithSuccess("Success!", "The order was successfully deleted.");
        }
    }
}