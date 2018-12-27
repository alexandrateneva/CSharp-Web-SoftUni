using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Services.Blog.Contracts;
using GrabNReadApp.Web.Areas.Blog.Models.Articles;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GrabNReadApp.Web.Areas.Blog.Controllers
{
    [Area("Blog")]
    public class ArticlesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IArticleService articleService;

        public ArticlesController(IMapper mapper, IArticleService articleService)
        {
            this.mapper = mapper;
            this.articleService = articleService;
        }

        // GET: Blog/Articles/Create
        [Authorize]
        public IActionResult Create()
        {
            return this.View();
        }

        // POST: Blog/Articles/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ArticleViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var article = mapper.Map<Article>(model);
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                article.AuthorId = userId;
                article.PublishedOn = DateTime.UtcNow;

                var result = await this.articleService.Create(article);

                return this.RedirectToAction("Index", "Home").WithSuccess("Success!", "The article was successfully created.");
            }

            return this.View(model);
        }

        // GET: Blog/Articles/All
        public IActionResult All()
        {
            var model = this.articleService
                .GetAllArticles()
                .Select(a => mapper.Map<ArticleBaseViewModel>(a))
                .ToList();

            if (User.IsInRole("Admin"))
            {
                return this.View(model);
            }

            var filteredModel = model.Where(a => a.IsApprovedByAdmin == true);
            return this.View(filteredModel);
        }

        // GET: Blog/Articles/Details/5
        public IActionResult Details(int id)
        {
            var article = this.articleService.GetArticleById(id);
            if (article == null)
            {
                var error = new Error() { Message = $"There is no article with id - {id}." };
                return this.View("CustomError", error);
            }

            var model = mapper.Map<ArticleDetailsViewModel>(article);
            return View(model);
        }

        // GET: Blog/Articles/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var article = this.articleService.GetArticleById(id);
            if (article == null)
            {
                var error = new Error() { Message = $"There is no article with id - {id}." };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName)
            {
                return this.RedirectToPage("Login")
                    .WithDanger("You were redirected!", "Please login, you don't have access rights.");
            }

            var model = mapper.Map<ArticleEditViewModel>(article);
            return View(model);
        }

        // POST: Blog/Articles/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ArticleEditViewModel model)
        {
            var article = this.articleService.GetArticleById(model.Id);
            if (article == null)
            {
                var error = new Error() { Message = $"There is no article with id - {model.Id}." };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName)
            {
                return this.RedirectToPage("Login")
                    .WithDanger("You were redirected!", "Please login, you don't have access rights.");
            }

            if (ModelState.IsValid)
            {
                article.Title = model.Title;
                article.Content = model.Content;

                var result = await this.articleService.Edit(article);

                return RedirectToAction("All", "Articles").WithSuccess("Success!", "The article was successfully edited.");
            }
            return this.View(model);
        }

        // GET: Blog/Articles/Delete/5
        [Authorize]
        public IActionResult Delete(int id)
        {
            var article = this.articleService.GetAllArticles().FirstOrDefault(g => g.Id == id);
            if (article == null)
            {
                var error = new Error() { Message = $"There is no article with id - {id}." };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName && !User.IsInRole("Admin"))
            {
                return this.RedirectToPage("Login")
                    .WithDanger("You were redirected!", "Please login, you don't have access rights.");
            }

            var model = mapper.Map<ArticleDeleteViewModel>(article);
            return this.View(model);
        }

        // POST: Blog/Articles/Delete/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var article = this.articleService.GetAllArticles().FirstOrDefault(g => g.Id == id);
            if (User.Identity.Name != article.Author.UserName && !User.IsInRole("Admin"))
            {
                return this.RedirectToPage("Login")
                    .WithDanger("You were redirected!", "Please login, you don't have access rights.");
            }

            var isDeleted = this.articleService.Delete(id);
            if (!isDeleted)
            {
                var error = new Error() { Message = "Delete failed." };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Articles").WithSuccess("Success!", "The article was successfully deleted.");
        }

        // GET: Blog/Articles/ChangeStatus/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var article = this.articleService.GetArticleById(id);
            if (article == null)
            {
                var error = new Error() { Message = $"There is no article with id - {id}." };
                return this.View("CustomError", error);
            }

            article.IsApprovedByAdmin = !article.IsApprovedByAdmin;
            var result = await this.articleService.Edit(article);

            return RedirectToAction("All", "Articles").WithSuccess("Success!", "You changed the article status successfully.");
        }
    }
}