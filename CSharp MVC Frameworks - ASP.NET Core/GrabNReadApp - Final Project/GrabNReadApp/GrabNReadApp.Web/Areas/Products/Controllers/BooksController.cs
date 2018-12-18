using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Web.Areas.Products.Models.Books;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GrabNReadApp.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class BooksController : Controller
    {
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly IBookService bookService;
        private readonly IConfiguration configuration;

        public BooksController(IMapper mapper, IGenreService genreService, IBookService bookService, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.genreService = genreService;
            this.bookService = bookService;
            this.configuration = configuration;
        }

        // GET: Products/Books/Create
        public ActionResult Create()
        {
            var genres = this.genreService.GetAllGenres();

            ViewBag.Genres = genres;
            return View();
        }

        // POST: Products/Books/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var extension = Path.GetExtension(model.CoverImageFile.Name);
                var fileName = Guid.NewGuid() + extension;

                using (var fileStream = model.CoverImageFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, fileStream),
                        Folder = "products"
                    };
                    
                    var apiKey = this.configuration["Cloudinary:ApiKey"];
                    var apiSecret = this.configuration["Cloudinary:ApiSecret"];
                    var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = "grabnreadapp" };
                    var cloudinary = new Cloudinary(myAccount);
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    model.CoverImage = uploadResult.Uri.AbsoluteUri;
                }

                var book = mapper.Map<Book>(model);
                var result = await this.bookService.Create(book);

                return RedirectToAction("Index", "Home");
            }

            return this.View(model);
        }
    }
}