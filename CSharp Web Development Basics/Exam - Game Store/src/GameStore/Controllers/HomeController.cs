namespace GameStore.Controllers
{
    using System;
    using System.Linq;
    using GameStore.ViewModels.Games;
    using GameStore.ViewModels.Home;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var games = this.Db.Games.Select(g => new BaseGameViewModel()
            {
                Id = g.Id,
                Title = g.Title,
                Trailer = g.Trailer,
                Description = TruncateLongString(g.Description, 30),
                Image = g.Image,
                ReleaseDate = g.ReleaseDate,
                Price = g.Price,
                Size = g.Size
            }).ToList();

            var model = new IndexViewModel()
            {
                Games = games
            };

            return this.View(model);
        }

        [Authorize]
        public IHttpResponse Owned()
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            var games = this.Db.Games
                .Where(x => x.Users.Any(ug => ug.UserId == user.Id))
                .Select(g => new BaseGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Trailer = g.Trailer,
                    Description = TruncateLongString(g.Description, 30),
                    Image = g.Image,
                    ReleaseDate = g.ReleaseDate,
                    Price = g.Price,
                    Size = g.Size
                }).ToList();

            var model = new IndexViewModel()
            {
                Games = games
            };

            return this.View("Home/Index", model);
        }

        private static string TruncateLongString(string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            return str.Substring(0, Math.Min(str.Length, maxLength));
        }
    }
}
