using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Services.Blog.Contracts;
using GrabNReadApp.Web.Areas.Blog.Models.Articles;
using GrabNReadApp.Web.Constants.Blog;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

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

                return this.RedirectToAction("Index", "Home").WithSuccess(ArticleConstants.SuccessMessageTitle, ArticleConstants.SuccessMessageForCreate);
            }

            return this.View(model);
        }

        // GET: Blog/Articles/All
        public IActionResult All(int? pageNumber)
        {
            var currentPage = pageNumber ?? ArticleConstants.FirstPageNumber;

            var articles = this.articleService
                .GetAllArticles()
                .Select(a => mapper.Map<ArticleBaseViewModel>(a))
                .OrderByDescending(x => x.PublishedOn);

            var filteredArticles = articles.Where(a => a.IsApprovedByAdmin == true);

            var model = new AllArticlesViewModel()
            {
                CurrentPage = currentPage,
                EmptyCollectionMessage = ArticleConstants.EmptyCollectionMessage
            };

            if (User.IsInRole("Admin"))
            {
                model.Articles = articles.ToPagedList(currentPage, ArticleConstants.ArticlesPerPage);
                return this.View(model);
            }

            model.Articles = filteredArticles.ToPagedList(currentPage, ArticleConstants.ArticlesPerPage);
            return this.View(model);
        }

        // GET: Blog/Articles/Details/5
        public IActionResult Details(int id)
        {
            var article = this.articleService.GetArticleById(id);
            if (article == null)
            {
                var error = new Error() { Message = string.Format(ArticleConstants.ErrorMessageForNotFound, id) };
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
                var error = new Error() { Message = string.Format(ArticleConstants.ErrorMessageForNotFound, id) };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName)
            {
                return this.RedirectToPage("/Account/Login", new { area = "Identity" })
                    .WithDanger(ArticleConstants.RedirectedMessageTitle, ArticleConstants.NotAccessMessage);
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
                var error = new Error() { Message = string.Format(ArticleConstants.ErrorMessageForNotFound, model.Id) };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName)
            {
                return this.RedirectToPage("/Account/Login", new { area = "Identity" })
                   .WithDanger(ArticleConstants.RedirectedMessageTitle, ArticleConstants.NotAccessMessage);
            }

            if (ModelState.IsValid)
            {
                article.IsApprovedByAdmin = false;
                article.Title = model.Title;
                article.Content = model.Content;

                var result = await this.articleService.Edit(article);

                return RedirectToAction("All", "Articles").WithSuccess(ArticleConstants.SuccessMessageTitle, ArticleConstants.SuccessMessageForEdit);
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
                var error = new Error() { Message = string.Format(ArticleConstants.ErrorMessageForNotFound, id) };
                return this.View("CustomError", error);
            }

            if (User.Identity.Name != article.Author.UserName && !User.IsInRole("Admin"))
            {
                return this.RedirectToPage("/Account/Login", new { area = "Identity" })
                     .WithDanger(ArticleConstants.RedirectedMessageTitle, ArticleConstants.NotAccessMessage);
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
                return this.RedirectToPage("/Account/Login", new { area = "Identity" })
                     .WithDanger(ArticleConstants.RedirectedMessageTitle, ArticleConstants.NotAccessMessage);
            }

            var isDeleted = this.articleService.Delete(id);
            if (!isDeleted)
            {
                var error = new Error() { Message = "Delete failed." };
                return this.View("CustomError", error);
            }
            return RedirectToAction("All", "Articles").WithSuccess(ArticleConstants.SuccessMessageTitle, ArticleConstants.SuccessMessageForDelete);
        }

        // GET: Blog/Articles/ChangeStatus/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ChangeStatus(int id)
        {
            var article = this.articleService.GetArticleById(id);
            if (article == null)
            {
                var error = new Error() { Message = string.Format(ArticleConstants.ErrorMessageForNotFound, id) };
                return this.View("CustomError", error);
            }

            article.IsApprovedByAdmin = !article.IsApprovedByAdmin;
            var result = await this.articleService.Edit(article);

            return RedirectToAction("All", "Articles").WithSuccess(ArticleConstants.SuccessMessageTitle, ArticleConstants.SuccessMessageForChangeStatus);
        }
    }
}