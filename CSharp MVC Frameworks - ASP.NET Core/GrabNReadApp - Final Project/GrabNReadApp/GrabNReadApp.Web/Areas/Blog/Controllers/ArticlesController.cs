using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
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
            return View();
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

                return RedirectToAction("Index", "Home").WithSuccess("Success!", "The article was successfully created.");
            }

            return this.View(model);
        }
    }
}