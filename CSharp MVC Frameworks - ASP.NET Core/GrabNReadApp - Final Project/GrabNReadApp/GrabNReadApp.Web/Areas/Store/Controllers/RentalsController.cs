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
        private readonly SignInManager<GrabNReadAppUser> signInManager;


        public RentalsController(IMapper mapper,
            IBookService bookService,
            IRentalsServices rentalsService,
            SignInManager<GrabNReadAppUser> signInManager)
        {
            this.mapper = mapper;
            this.bookService = bookService;
            this.rentalsService = rentalsService;
            this.signInManager = signInManager;
        }

        // GET: Store/Rentals/Create/id
        public async Task<IActionResult> Create(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login").WithWarning("You were redirected!", "To make order, please first Login!");
            }

            var book = await this.bookService.GetBookById(id);
            var model = new RentalViewModel()
            {
                BookId = id,
                Book = book,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1),
                TotalSum = book.PricePerDay * 1
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

            if (DateTime.Now > model.StartDate)
            {
                return this.View(model).WithDanger("Error!", "You can not rent a book for a past times.");
            }
            else if (model.StartDate >= model.EndDate)
            {
                return this.View(model).WithDanger("Error!", "End date must be greater than the start date.");
            }
            else if ((model.EndDate - model.StartDate).Days > 30)
            {
                return this.View(model).WithDanger("Error!", "You can rent the book for a maximum period of 30 days.");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.CustomerId = userId;

            if (this.ModelState.IsValid)
            {
                var rental = mapper.Map<Rental>(model);
                var result = await this.rentalsService.Create(rental);

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess("Success!", "Тhe book has been successfully added to your cart.");
            }

            return this.View(model);
        }
    }
}