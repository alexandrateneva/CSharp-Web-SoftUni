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
using GrabNReadApp.Web.Areas.Products.Models.Genres;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GrabNReadApp.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class GenresController : Controller
    {
        private readonly IMapper mapper;
        private readonly IGenreService genreService;
        private readonly IConfiguration configuration;

        public GenresController(IMapper mapper, IGenreService genreService, IConfiguration configuration)
        {
            this.mapper = mapper;
            this.genreService = genreService;
            this.configuration = configuration;
        }

        // GET: Products/Genres/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Genres/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GenreViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var extension = Path.GetExtension(model.ImageFile.Name);
                var fileName = Guid.NewGuid() + extension;

                using (var fileStream = model.ImageFile.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(fileName, fileStream),
                        Folder = "categories"
                    };

                    var apiKey = this.configuration["Cloudinary:ApiKey"];
                    var apiSecret = this.configuration["Cloudinary:ApiSecret"];
                    var myAccount = new Account { ApiKey = apiKey, ApiSecret = apiSecret, Cloud = "grabnreadapp" };
                    var cloudinary = new Cloudinary(myAccount);
                    var uploadResult = await cloudinary.UploadAsync(uploadParams);

                    model.Image = uploadResult.Uri.AbsoluteUri;
                }

                var genre = mapper.Map<Genre>(model);
                var result = await this.genreService.Create(genre);

                return RedirectToAction("Index", "Home");
            }

            return this.View(model);
        }
    }
}