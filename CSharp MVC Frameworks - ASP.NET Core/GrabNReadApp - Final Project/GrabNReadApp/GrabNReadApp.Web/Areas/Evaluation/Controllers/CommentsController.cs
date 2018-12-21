using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
using GrabNReadApp.Web.Areas.Evaluation.Models.Comments;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GrabNReadApp.Web.Areas.Evaluation.Controllers
{
    [Area("Evaluation")]
    public class CommentsController : Controller
    {
        private readonly IMapper mapper;
        private readonly ICommentsService commentsService;
        private readonly SignInManager<GrabNReadAppUser> signInManager;

        public CommentsController(IMapper mapper, ICommentsService commentsService, SignInManager<GrabNReadAppUser> signInManager)
        {
            this.mapper = mapper;
            this.commentsService = commentsService;
            this.signInManager = signInManager;
        }

        // GET: Evaluation/Comments/Create
        [Authorize]
        public IActionResult Create()
        {
            return RedirectToAction("All", "Books", new { area = "Products" })
                .WithSuccess("You are logged in.", "Now you can comment our books!");
        }

        // POST: Evaluation/Comments/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CommentViewModel model)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            model.CreatorId = userId;

            if (ModelState.IsValid)
            {
                var comment = mapper.Map<Comment>(model);
                var result = await this.commentsService.Create(comment);

                return Ok();
                //return RedirectToAction("Details", "Books", new { area = "Products", id = model.BookId })
                //    .WithSuccess("Success!", "Your comment has been successfully added.");
            }

            return RedirectToAction("Details", "Books", new { area = "Products", id = model.BookId })
                .WithDanger("Please add valid content!", "Your comment must be between 5 and 200 symbols.");
        }
    }
}