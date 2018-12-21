using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Web.Areas.Evaluation.Models.Comments;
using GrabNReadApp.Web.Extensions.Alerts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        private readonly IBookService bookService;

        public CommentsController(IMapper mapper,
            ICommentsService commentsService,
            SignInManager<GrabNReadAppUser> signInManager,
            IBookService bookService)
        {
            this.mapper = mapper;
            this.commentsService = commentsService;
            this.signInManager = signInManager;
            this.bookService = bookService;
        }

        // POST: Evaluation/Comments/Create
        [HttpPost]
        public async Task<IActionResult> Create(string content, int bookId)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Json(new { authorize = "Failed" });
            }

            if (await this.bookService.GetBookById(bookId) == null)
            {
                return Json(new { bookValidation = "Failed" });
            }

            if (string.IsNullOrEmpty(content))
            {
                return Json(new { contentValidation = "Failed" });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var comment = new Comment()
            {
                Content = content,
                BookId = bookId,
                CreatorId = userId
            };
            var result = await this.commentsService.Create(comment);

            return Json(true);
        }
    }
}