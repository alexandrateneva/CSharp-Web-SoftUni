using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Web.Areas.Products.Models.Genres;
using GrabNReadApp.Web.Helper;
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
                var apiKey = configuration["Cloudinary:ApiKey"];
                var apiSecret = configuration["Cloudinary:ApiSecret"];

                model.Image = await CloudinaryFileUploader.UploadFile(model.ImageFile, "categories", apiKey, apiSecret);

                var genre = mapper.Map<Genre>(model);
                var result = await this.genreService.Create(genre);

                return RedirectToAction("All", "Genres");
            }

            return this.View(model);
        }

        // GET: Products/Genres/All
        public ActionResult All()
        {
            var genres = this.genreService.GetAllGenres()
                .Select(g => mapper.Map<GenreBaseViewModel>(g))
                .ToList();

            return View(genres);
        }

        // GET: Products/Genres/Edit/5
        public ActionResult Edit(int id)
        {
            var genre = this.genreService.GetAllGenres().FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                var error = new Error() { Message = $"There is no genre with id - {id}." };
                return this.View("CustomError", error);
            }
            var model = mapper.Map<GenreEditViewModel>(genre);
            return View(model);
        }

        // POST: Products/Genres/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(GenreEditViewModel model)
        {
            if (this.ModelState.IsValid && (model.ImageFile != null || model.Image != null))
            {
                if (model.ImageFile != null)
                {
                    var apiKey = configuration["Cloudinary:ApiKey"];
                    var apiSecret = configuration["Cloudinary:ApiSecret"];

                    model.Image = await CloudinaryFileUploader.UploadFile(model.ImageFile, "categories", apiKey, apiSecret);
                }

                var genre = mapper.Map<Genre>(model);
                var result = await this.genreService.Edit(genre);

                return RedirectToAction("All", "Genres");
            }

            return this.View(model);
        }

        // GET: Products/Genres/Delete/5
        public ActionResult Delete(int id)
        {
            var genre = this.genreService.GetAllGenres().FirstOrDefault(g => g.Id == id);
            if (genre == null)
            {
                var error = new Error() { Message = $"There is no genre with id - {id}." };
                return this.View("CustomError", error);
            }
            var model = mapper.Map<GenreDeleteViewModel>(genre);
            return this.View(model);
        }

        // POST: Products/Genres/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(GenreDeleteViewModel model)
        {
            var isDeleted = this.genreService.Delete(model.Id);
            if (isDeleted.Status == TaskStatus.Faulted)
            {
                var error = new Error() { Message = "Delete failed." };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Genres");
        }
    }
}