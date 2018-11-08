namespace JudgeSystem.Controllers
{
    using System;
    using System.Linq;
    using JudgeSystem.Models;
    using JudgeSystem.ViewModels.Contests;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class ContestsController : BaseController
    {
        [Authorize]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(CreateContestInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return this.BadRequestErrorWithView("Please provide valid name of contest.");
            }

            var contest = this.Db.Contests.FirstOrDefault(x => x.Name == model.Name);
            if (contest != null)
            {
                return this.BadRequestErrorWithView("Contest with this name already exists.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            var newContest = new Contest()
            {
                Name = model.Name,
                CreatorId = user.Id
            };

            this.Db.Contests.Add(newContest);
            this.Db.SaveChanges();

            return this.Redirect("/Contests/All");
        }

        [Authorize]
        public IHttpResponse All()
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            var contests = this.Db.Contests.Select(c => new BaseContestViewModel()
            {
                Id = c.Id,
                Name = c.Name,
                HasLoggedInCreator = user.Id == c.CreatorId,
                SubmissionsCount = c.Submissions.Count
            }).ToList();

            var model = new AllContestsViewModel()
            {
                Contests = contests
            };

            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Delete(int id)
        {
            var contest = this.Db.Contests.FirstOrDefault(c => c.Id == id);
            if (contest == null)
            {
                return this.BadRequestError("This contest does not exist.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);
            if (contest.CreatorId != user.Id && this.User.Role != "Admin")
            {
                return this.BadRequestError("Only contest's creator or admin can edit it.");
            }

            var model = new BaseContestViewModel()
            {
                Id = id,
                Name = contest.Name
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Delete(BaseContestViewModel model)
        {
            var contest = this.Db.Contests.FirstOrDefault(c => c.Id == model.Id);
            if (contest == null)
            {
                return this.BadRequestError("This contest does not exist.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);
            if (contest.CreatorId != user.Id && this.User.Role != "Admin")
            {
                return this.BadRequestError("Only contest's creator or admin can delete it.");
            }

            this.Db.Contests.Remove(contest);
            this.Db.SaveChanges();

            return this.Redirect("/Contests/All");
        }

        [Authorize]
        public IHttpResponse Edit(int id)
        {
            var contest = this.Db.Contests.FirstOrDefault(c => c.Id == id);
            if (contest == null)
            {
                return this.BadRequestError("This contest does not exist.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);
            if (contest.CreatorId != user.Id && this.User.Role != "Admin")
            {
                return this.BadRequestError("Only contest's creator or admin can edit it.");
            }

            var model = new BaseContestViewModel()
            {
                Id = id,
                Name = contest.Name
            };

            return this.View(model);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Edit(BaseContestViewModel model)
        {
            var contest = this.Db.Contests.FirstOrDefault(c => c.Id == model.Id);
            if (contest == null)
            {
                return this.BadRequestError("This contest does not exist.");
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);
            if (contest.CreatorId != user.Id && this.User.Role != "Admin")
            {
                return this.BadRequestError("Only contest's creator or admin can edit it.");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                return this.BadRequestErrorWithView("Please provide valid name.");
            }

            contest.Name = model.Name;

            this.Db.Contests.Update(contest);
            this.Db.SaveChanges();

            return this.Redirect("/Contests/All");
        }
    }
}
