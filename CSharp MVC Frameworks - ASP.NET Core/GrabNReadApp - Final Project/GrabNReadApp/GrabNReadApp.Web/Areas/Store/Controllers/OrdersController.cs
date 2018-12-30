using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using GrabNReadApp.Web.Areas.Store.Models.Orders;
using GrabNReadApp.Web.Constants.Store;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess(OrdersConstants.OrderSuccessMessageTitle, OrdersConstants.SuccessMessageForOrder);
            }

            return this.View(model);
        }

        // GET: Store/Orders/All
        [Authorize(Roles = "Admin")]
        public IActionResult All(int? pageNumber)
        {
            var currentPage = pageNumber ?? OrdersConstants.FirstPageNumber;
            var orders = this.ordersService.GetAllFinishedOrders().OrderByDescending(o => o.OrderedOn);

            var onePageOfEvents = orders.ToPagedList(currentPage, OrdersConstants.OrdersPerPage);

            var model = new AllOrdersViewModel()
            {
                Orders = onePageOfEvents,
                CurrentPage = currentPage
            };

            return View(model);
        }

        // GET: Store/Orders/Details/5
        [Authorize(Roles = "Admin")]
        public IActionResult Details(int id)
        {
            var order = this.ordersService.GetOrderByIdWithPurchasesAndRentals(id);
            if (order == null)
            {
                var error = new Error() { Message = string.Format(OrdersConstants.ErrorMessageForNotFound, id) };
                return this.View("CustomError", error);
            }

            var model = mapper.Map<OrderDetailsViewModel>(order);
            return View(model);
        }

        // GET: Store/Orders/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var isDeleted = this.ordersService.Delete(id);
            if (!isDeleted)
            {
                var error = new Error() { Message = OrdersConstants.ErrorMessageForDelete };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Orders").WithSuccess(OrdersConstants.SuccessMessageTitle, OrdersConstants.SuccessMessageForDelete);
        }
    }
}