using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Data.Services.Store.Contracts;
using GrabNReadApp.Web.Areas.Store.Models.Rentals;
using GrabNReadApp.Web.Constants.Store;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GrabNReadApp.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class RentalsController : Controller
    {
        private readonly IMapper mapper;
        private readonly IBookService bookService;
        private readonly IRentalsServices rentalsService;
        private readonly IOrdersService ordersService;
        private readonly SignInManager<GrabNReadAppUser> signInManager;


        public RentalsController(IMapper mapper,
            IBookService bookService,
            IRentalsServices rentalsService,
            IOrdersService ordersService,
            SignInManager<GrabNReadAppUser> signInManager)
        {
            this.mapper = mapper;
            this.bookService = bookService;
            this.rentalsService = rentalsService;
            this.ordersService = ordersService;
            this.signInManager = signInManager;
        }

        // GET: Store/Rentals/Create/id
        public async Task<IActionResult> Create(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login").WithWarning(RentalsConstants.RedirectedMessageTitle, RentalsConstants.RedirectedMessage);
            }

            var book = await this.bookService.GetBookById(id);
            var model = new RentalViewModel()
            {
                BookId = id,
                Book = book,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(RentalsConstants.DefaultPeriodInDays),
                TotalSum = book.PricePerDay * RentalsConstants.DefaultCount
            };
            return View(model);
        }

        // POST: Store/Rentals/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RentalViewModel model)
        {
            var book = await this.bookService.GetBookById(model.BookId);
            model.Book = book;

            if (DateTime.Today > model.StartDate)
            {
                return this.View(model).WithDanger(RentalsConstants.ErrorMessageTitle, RentalsConstants.PastTimeErrorMessage);
            }
            else if (model.StartDate >= model.EndDate)
            {
                return this.View(model).WithDanger(RentalsConstants.ErrorMessageTitle, RentalsConstants.StartDateGreaterThanEndDateErrorMessage);
            }
            else if ((model.EndDate - model.StartDate).Days > RentalsConstants.MaximumPeriodInDays)
            {
                return this.View(model).WithDanger(RentalsConstants.ErrorMessageTitle, string.Format(RentalsConstants.MaximumPeriodErrorMessage, RentalsConstants.MaximumPeriodInDays));
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.CustomerId = userId;
            var order = this.ordersService.GetCurrentOrderByUserIdWithPurchasesAndRentals(userId);
            model.OrderId = order.Id;

            if (this.ModelState.IsValid)
            {
                var rental = mapper.Map<Rental>(model);
                var result = await this.rentalsService.Create(rental);

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess(RentalsConstants.SuccessMessageTitle, RentalsConstants.SuccessfullyAddedToCartMessage);
            }

            return this.View(model);
        }

        // GET: Store/Rentals/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var rental = await this.rentalsService.GetRentalById(id);

            if (rental.CustomerId != userId && !User.IsInRole("Admin"))
            {
                return Redirect("/Identity/Account/Login").WithDanger(RentalsConstants.RedirectedMessageTitle,
                    RentalsConstants.NotAccessMessage);
            }

            var rentalDelete = this.rentalsService.Delete(id);
            if (!rentalDelete)
            {
                var error = new Error() { Message = string.Format(RentalsConstants.ErrorMessageForNotFound, id) };
                return this.View("CustomError", error);
            }

            var referer = Request.Headers["Referer"].ToString();
            return Redirect(referer).WithSuccess(RentalsConstants.SuccessMessageTitle, RentalsConstants.SuccessMessageForDelete);
        }
    }
}