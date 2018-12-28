﻿using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Services.Evaluation.Contracts;
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

        public RatingsController(IMapper mapper,
            SignInManager<GrabNReadAppUser> signInManager,
            IRatingService ratingsService)
        {
            this.mapper = mapper;
            this.signInManager = signInManager;
            this.ratingsService = ratingsService;
        }

        // POST: Evaluation/Ratings/Vote
        [HttpPost]
        public async Task<IActionResult> Vote(int bookId, int voteValue)
        {
            if (!signInManager.IsSignedIn(User))
            {
                return Json(new { authorize = "Failed" });
            }

            if (voteValue < 1 || voteValue > 5)
            {
                return Json(new { voteValidation = "Failed" });
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