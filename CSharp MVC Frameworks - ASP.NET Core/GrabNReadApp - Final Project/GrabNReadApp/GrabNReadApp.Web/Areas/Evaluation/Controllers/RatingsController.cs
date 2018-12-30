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
    public class RatingsController : Controller
    {
        private readonly IMapper mapper;
        private readonly SignInManager<GrabNReadAppUser> signInManager;
        private readonly IRatingService ratingsService;
        private readonly IBookService bookService;

        public RatingsController(IMapper mapper,
            SignInManager<GrabNReadAppUser> signInManager,
            IRatingService ratingsService,
            IBookService bookService)
        {
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.ratingsService = ratingsService;
            this.bookService = bookService;
        }

        // POST: Evaluation/Ratings/Vote
        [HttpPost]
        public async Task<IActionResult> Vote(int bookId, int voteValue)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Json(new { authorize = "Failed" });
            }

            var book =  await this.bookService.GetBookById(bookId);
            if (book == null)
            {
                return Json(new { bookValidation = "Failed", bookValidationMsg = RatingsConstants.BookNotFoundMessage });
            }

            if (voteValue < RatingsConstants.VoteMinValue || voteValue > RatingsConstants.VoteMaxValue)
            {
                var message = string.Format(RatingsConstants.VoteValueValidationMessage,
                    RatingsConstants.VoteMinValue, RatingsConstants.VoteMaxValue);
                return Json(new { voteValidation = "Failed", voteValidationMsg = message });
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var previousVote = this.ratingsService.GetVoteByUserIdAndBookId(userId, bookId);
            if (previousVote != null)
            {
                previousVote.VoteValue = voteValue;
                var result = await this.ratingsService.ChangeVote(previousVote);
            }
            else
            {
                var vote = new Vote()
                {
                    BookId = bookId,
                    CreatorId = userId,
                    VoteValue = voteValue
                };
                var result = await this.ratingsService.Create(vote);
            }

            var averageRating = this.ratingsService.GetAverageBookRatingByBookId(bookId);
            return Json(new { averageRating = averageRating });
        }
    }
}