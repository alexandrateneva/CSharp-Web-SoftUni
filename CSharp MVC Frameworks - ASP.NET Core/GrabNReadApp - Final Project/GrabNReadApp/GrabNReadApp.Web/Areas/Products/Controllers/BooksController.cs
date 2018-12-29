using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Web.Areas.Evaluation.Models.Comments;
using GrabNReadApp.Web.Areas.Products.Models.Books;
using GrabNReadApp.Web.Extensions.Alerts;
using GrabNReadApp.Web.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using X.PagedList;

namespace GrabNReadApp.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class BooksController : Controller
    {
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly IBookService bookService;
        private readonly IConfiguration configuration;
        private readonly ICommentsService commentsService;
        private readonly IRatingService ratingService;

        public BooksController(IMapper mapper,
            IGenreService genreService,
            IBookService bookService,
            IConfiguration configuration,
            ICommentsService commentsService,
            IRatingService ratingService)
        {
            this.mapper = mapper;
            this.genreService = genreService;
            this.bookService = bookService;
            this.configuration = configuration;
            this.commentsService = commentsService;
            this.ratingService = ratingService;
        }

        // GET: Products/Books/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var genres = this.genreService.GetAllGenres();
            ViewBag.Genres = genres;
            return View();
        }

        // POST: Products/Books/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var apiKey = configuration["Cloudinary:ApiKey"];
                var apiSecret = configuration["Cloudinary:ApiSecret"];

                model.CoverImage = await CloudinaryFileUploader.UploadFile(model.CoverImageFile, "products", apiKey, apiSecret);

                var book = mapper.Map<Book>(model);
                var result = await this.bookService.Create(book);

                return RedirectToAction("All", "Books").WithSuccess("Success!", "The book was successfully created.");
            }

            return this.View(model);
        }

        // GET: Products/Books/All
        public IActionResult All(int? pageNumber)
        {
            var currentPage = pageNumber ?? 1;
            var books = this.bookService.GetAllBooks()
                .Select(b => mapper.Map<BookBaseViewModel>(b));

            var onePageOfEvents = books.ToPagedList(currentPage, 4);

            var model = new AllBooksViewModel()
            {
                Books = onePageOfEvents,
                CurrentPage = currentPage
            };

            return View(model);
        }

        [Route("Products/Books/Genre/{id:int}")]
        public IActionResult AllByGenre(int id, int? pageNumber)
        {
            var currentPage = pageNumber ?? 1;
            var books = this.bookService.GetAllBooks()
                .Where(b => b.GenreId == id)
                .Select(b => mapper.Map<BookBaseViewModel>(b));

            var onePageOfEvents = books.ToPagedList(currentPage, 4);

            var model = new AllBooksViewModel()
            {
                Books = onePageOfEvents,
                CurrentPage = currentPage,
                GenreId = id
            };

            return View("AllBooksByGenre", model);
        }

        // GET: Products/Books/Search
        public IActionResult Search(string bookTitle, int? pageNumber)
        {
            if (!string.IsNullOrEmpty(bookTitle))
            {
                var currentPage = pageNumber ?? 1;
                var books = this.bookService.GetBooksByTitle(bookTitle)
                    .Select(b => mapper.Map<BookBaseViewModel>(b));

                var onePageOfEvents = books.ToPagedList(currentPage, 4);

                var viewModel = new AllBooksViewModel()
                {
                    Books = onePageOfEvents,
                    CurrentPage = currentPage,
                    BookTitle = bookTitle
                };

                return View("AllBooksByTitleSearch", viewModel);
            }

            return this.RedirectToAction("All", "Books").WithDanger("Book title is required!", "To search, please enter a book title.");
        }

        // GET: Products/Books/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var genres = this.genreService.GetAllGenres();
            ViewBag.Genres = genres;

            var book = this.bookService.GetAllBooks().FirstOrDefault(g => g.Id == id);
            if (book == null)
            {
                var error = new Error() { Message = $"There is no book with id - {id}." };
                return this.View("CustomError", error);
            }
            var model = mapper.Map<BookEditViewModel>(book);
            return View(model);
        }

        // POST: Products/Books/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(BookEditViewModel model)
        {
            if (this.ModelState.IsValid && (model.CoverImageFile != null || model.CoverImage != null))
            {
                if (model.CoverImageFile != null)
                {
                    var apiKey = configuration["Cloudinary:ApiKey"];
                    var apiSecret = configuration["Cloudinary:ApiSecret"];

                    model.CoverImage = await CloudinaryFileUploader.UploadFile(model.CoverImageFile, "products", apiKey, apiSecret);
                }

                var book = mapper.Map<Book>(model);
                var result = await this.bookService.Edit(book);

                return RedirectToAction("All", "Books").WithSuccess("Success!", "The book was successfully edited."); 
            }

            return this.View(model);
        }

        // GET: Products/Books/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var book = this.bookService.GetAllBooks().FirstOrDefault(g => g.Id == id);
            if (book == null)
            {
                var error = new Error() { Message = $"There is no book with id - {id}." };
                return this.View("CustomError", error);
            }
            var model = mapper.Map<BookDeleteViewModel>(book);
            return this.View(model);
        }

        // POST: Products/Books/Delete/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var isDeleted = this.bookService.Delete(id);
            if (!isDeleted)
            {
                var error = new Error() { Message = "Delete failed." };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Books").WithSuccess("Success!", "The book was successfully deleted."); 
        }

        // GET: Products/Books/Details/5
        public IActionResult Details(int id)
        {
            var book = this.bookService.GetAllBooks().FirstOrDefault(g => g.Id == id);
            if (book == null)
            {
                var error = new Error() { Message = $"There is no book with id - {id}." };
                return this.View("CustomError", error);
            }

            var model = mapper.Map<BookDetailsViewModel>(book);

            model.UserVote = 0;
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                var userVote = this.ratingService.GetVoteValueByUserIdAndBookId(userId, id);
                model.UserVote = userVote;
            }

            model.AverageRating = this.ratingService.GetAverageBookRatingByBookId(id);

            var commentViewModel = new CommentViewModel()
            {
                BookId = book.Id
            };
            model.CommentViewModel = commentViewModel;

            var comments = this.commentsService.GetAllCommentsByBookId(id);
            model.Comments = comments;

            return this.View(model);
        }
    }
}