namespace JudgeSystem.Controllers
{
    using System;
    using System.Linq;
    using JudgeSystem.Models;
    using JudgeSystem.ViewModels.Contests;
    using JudgeSystem.ViewModels.Submissions;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class SubmissionsController : BaseController
    {
        [Authorize]
        public IHttpResponse All()
        {
            var contests = this.Db.Contests.Select(c => new BaseContestViewModel()
            {
                Id = c.Id,
                Name = c.Name,
            }).ToList();

            var model = new AllSubmissionsViewModel()
            {
                Contests = contests
            };

            return this.View(model);
        }

        [Authorize]
        public IHttpResponse ByContestId(int id)
        {
            var contests = this.Db.Contests
                .Select(c => new BaseContestViewModel()
                {
                    Id = c.Id,
                    Name = c.Name,
                }).ToList();

            var submissions = this.Db.Submissions
                .Where(x => x.ContestId == id)
                .Select(s => new BaseSubmissionViewModel()
                {
                    IsSuccessful = s.IsSuccessful
                }).ToList();


            var model = new AllSubmissionsViewModel()
            {
                Contests = contests,
                Submissions = submissions
            };
            
            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Create()
        {
            var contests = this.Db.Contests
                .Select(c => new BaseContestViewModel()
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList();

            var model = new CreateSubmissionViewModel()
            {
                Contests = contests
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(CreateSubmissionInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Code))
            {
                return this.BadRequestErrorWithView("Please provide valid code.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            var random = new Random();
            var perCent = random.Next(0, 100);
            var isSuccessful = perCent >= 70;

            var newSubmission = new Submission()
            {
                Code = model.Code,
                CreatorId = user.Id,
                ContestId = model.ContestId,
                IsSuccessful = isSuccessful
            };

            this.Db.Submissions.Add(newSubmission);
            this.Db.SaveChanges();

            return this.Redirect("/Submissions/All");
        }
    }
}
