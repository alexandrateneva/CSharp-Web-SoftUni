using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
using GrabNReadApp.Data.Services.Products.Contracts;
using GrabNReadApp.Web.Constants.Evaluation;
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
                var message = CommentsConstants.BookNotFoundMessage;
                return Json(new { bookValidation = "Failed", bookValidationMsg = message });
            }

            if (string.IsNullOrEmpty(content) || content.Length < CommentsConstants.ContentMinLength || content.Length > CommentsConstants.ContentMaxLength)
            {
                var message = string.Format(CommentsConstants.CommentLengthValidationMessage,
                    CommentsConstants.ContentMinLength, CommentsConstants.ContentMaxLength);
                return Json(new { contentValidation = "Failed", contentValidationMsg = message });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var comment = new Comment()
            {
                Content = content,
                BookId = bookId,
                CreatorId = userId
            };
            var result = await this.commentsService.Create(comment);

            return Json(new { commentId = result.Id });
        }

        // POST: Evaluation/Comments/Delete
        [HttpPost]
        public IActionResult Delete(int commentId)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Json(new { authorize = "Failed" });
            }

            var comment = commentsService.GetCommentById(commentId);

            if (comment == null)
            {
                var message = CommentsConstants.CommentNotFoundMessage;
                return Json(new { commentValidation = "Failed", commentValidationMsg = message });
            }

            if (comment.Creator.UserName != User.Identity.Name && !User.IsInRole("Admin"))
            {
                return Json(new { authorize = "Failed" });
            }

            this.commentsService.Delete(commentId);

            return Json(true);
        }
    }
}