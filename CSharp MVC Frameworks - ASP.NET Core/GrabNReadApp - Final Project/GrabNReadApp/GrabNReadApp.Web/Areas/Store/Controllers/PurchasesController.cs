﻿using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Data.Services.Store.Contracts;
using GrabNReadApp.Web.Areas.Store.Models.Purchases;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GrabNReadApp.Web.Areas.Store.Controllers
{
    [Area("Store")]
    public class PurchasesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IBookService bookService;
        private readonly IPurchasesService purchasesService;
        private readonly SignInManager<GrabNReadAppUser> signInManager;


        public PurchasesController(IMapper mapper,
            IBookService bookService,
            IPurchasesService purchasesService,
            SignInManager<GrabNReadAppUser> signInManager)
        {
            this.mapper = mapper;
            this.bookService = bookService;
            this.purchasesService = purchasesService;
            this.signInManager = signInManager;
        }

        // GET: Store/Purchases/Create/id
        public async Task<IActionResult> Create(int id)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Redirect("/Identity/Account/Login").WithWarning("You were redirected!", "To make order, please first Login!");
            }

            var book = await this.bookService.GetBookById(id);
            var model = new PurchaseViewModel()
            {
                BookId = id,
                Book = book,
                BookCount = 1,
                TotalSum = book.Price * 1
            };
            return View(model);
        }

        // POST: Store/Purchases/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PurchaseViewModel model)
        {
            var book = await this.bookService.GetBookById(model.BookId);
            model.Book = book;
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.CustomerId = userId;

            if (this.ModelState.IsValid)
            {
                var purchase = mapper.Map<Purchase>(model);
                var result = await this.purchasesService.Create(purchase);

                return RedirectToAction("All", "Books", new { area = "Products" }).WithSuccess("Success!", "Тhe book has been successfully added to your cart.");
            }

            return this.View(model);
        }
    }
}