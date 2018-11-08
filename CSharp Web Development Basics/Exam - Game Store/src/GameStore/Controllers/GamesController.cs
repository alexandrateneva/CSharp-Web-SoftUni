namespace GameStore.Controllers
{
    using System;
    using System.Linq;
    using GameStore.Models;
    using GameStore.ViewModels.Games;
    using GameStore.ViewModels.Home;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class GamesController: BaseController
    {
        public IHttpResponse Details(int id)
        {
            var gameViewModel = this.Db.Games.Where(x => x.Id == id)
                .Select(g => new BaseGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Trailer = g.Trailer,
                    Description = g.Description,
                    Image = g.Image,
                    ReleaseDate = g.ReleaseDate,
                    Price = g.Price,
                    Size = g.Size
                }).FirstOrDefault();

            if (gameViewModel == null)
            {
                return this.BadRequestError("Invalid game id.");
            }

            return this.View(gameViewModel);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Buy(IdGameInputModel model)
        {
            var user = this.Db.Users.FirstOrDefault(u => u.Email == this.User.Info);

            var userGame = new UserGame()
            {
                UserId = user.Id,
                GameId = model.Id
            };

            if (this.Db.UsersGames.Any(ug => ug.UserId == user.Id && ug.GameId == model.Id))
            {
                return this.BadRequestError("You already have this game.");
            }

            this.Db.UsersGames.Add(userGame);
            this.Db.SaveChanges();

            return this.Redirect("/Home/Owned");
        }

        [Authorize]
        public IHttpResponse Create()
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Create(CreateGameInputModel model)
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrEmpty(model.Title) || !Char.IsUpper(model.Title[0]) && model.Title.Length < 3 && model.Title.Length > 100)
            {
                return this.BadRequestErrorWithView("Title has to begin with uppercase letter and has length between 3 and 100 symbols (inclusive).");
            }

            if (model.Price <= 0)
            {
                return this.BadRequestErrorWithView("Price must be a positive number.");
            }

            if (model.Size <= 0)
            {
                return this.BadRequestErrorWithView("Size must be a positive number.");
            }
            
            if (string.IsNullOrEmpty(model.Image) || (!model.Image.StartsWith("http://") && !model.Image.StartsWith("https://")))
            {
                return this.BadRequestErrorWithView("Thumbnail URL should be a plain text starting with http:// or https://.");
            }

            if (string.IsNullOrEmpty(model.Trailer) || model.Trailer.StartsWith("http://") || model.Trailer.StartsWith("https://"))
            {
                return this.BadRequestErrorWithView(@"Trailer – only videos from YouTube are allowed and only their ID should be get which is a text of exactly 11 characters. 
                For example, if the URL to the trailer is https://www.youtube.com/watch?v=edYCtaNueQY, the required part is edYCtaNueQY.");
            }

            if (model.Trailer.Length != 11)
            {
                return this.BadRequestErrorWithView(@"Trailer – only videos from YouTube are allowed and only their ID should be get which is a text of exactly 11 characters. 
                For example, if the URL to the trailer is https://www.youtube.com/watch?v=edYCtaNueQY, the required part is edYCtaNueQY.");
            }

            if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 20)
            {
                return this.BadRequestErrorWithView("Description must be at least 20 symbols.");
            }

            var game = new Game()
            {
                Title = model.Title,
                Trailer = model.Trailer,
                Description = model.Description,
                Image = model.Image,
                ReleaseDate = model.ReleaseDate,
                Price = model.Price,
                Size = model.Size
            };

            this.Db.Games.Add(game);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse All()
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            var games = this.Db.Games.Select(g => new BaseGameViewModel()
            {
                Id = g.Id,
                Title = g.Title,
                Trailer = g.Trailer,
                Description = g.Description,
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
        public IHttpResponse Edit(int id)
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            var gameViewModel = this.Db.Games
                .Where(x => x.Id == id)
                .Select(g => new BaseGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title,
                    Trailer = g.Trailer,
                    Description = g.Description,
                    Image = g.Image,
                    ReleaseDate = g.ReleaseDate,
                    Price = g.Price,
                    Size = g.Size
                }).FirstOrDefault();

            if (gameViewModel == null)
            {
                return this.BadRequestError("Invalid game id.");
            }

            return this.View(gameViewModel);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Edit(BaseGameViewModel model)
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            var game = this.Db.Games.FirstOrDefault(x => x.Id == model.Id);

            if (game == null)
            {
                return this.BadRequestError("Invalid game id.");
            }

            if (string.IsNullOrEmpty(model.Title) || !Char.IsUpper(model.Title[0]) && model.Title.Length < 3 && model.Title.Length > 100)
            {
                return this.BadRequestErrorWithView("Title has to begin with uppercase letter and has length between 3 and 100 symbols (inclusive).");
            }

            if (model.Price <= 0)
            {
                return this.BadRequestErrorWithView("Price must be a positive number.");
            }

            if (model.Size <= 0)
            {
                return this.BadRequestErrorWithView("Size must be a positive number.");
            }

            if (string.IsNullOrEmpty(model.Image) || (!model.Image.StartsWith("http://") && !model.Image.StartsWith("https://")))
            {
                return this.BadRequestErrorWithView("Thumbnail URL should be a plain text starting with http:// or https://.");
            }

            if (string.IsNullOrEmpty(model.Trailer) || model.Trailer.StartsWith("http://") || model.Trailer.StartsWith("https://"))
            {
                return this.BadRequestErrorWithView(@"Trailer – only videos from YouTube are allowed and only their ID should be get which is a text of exactly 11 characters. 
                For example, if the URL to the trailer is https://www.youtube.com/watch?v=edYCtaNueQY, the required part is edYCtaNueQY.");
            }

            if (model.Trailer.Length != 11)
            {
                return this.BadRequestErrorWithView(@"Trailer – only videos from YouTube are allowed and only their ID should be get which is a text of exactly 11 characters. 
                For example, if the URL to the trailer is https://www.youtube.com/watch?v=edYCtaNueQY, the required part is edYCtaNueQY.");
            }

            if (string.IsNullOrEmpty(model.Description) || model.Description.Length < 20)
            {
                return this.BadRequestErrorWithView("Description must be at least 20 symbols.");
            }

            game.Title = model.Title;
            game.Trailer = model.Trailer;
            game.Description = model.Description;
            game.Image = model.Image;
            game.ReleaseDate = model.ReleaseDate;
            game.Price = model.Price;
            game.Size = model.Size;

            this.Db.Games.Update(game);
            this.Db.SaveChanges();

            return this.Redirect("/Games/All");
        }

        [Authorize]
        public IHttpResponse Delete(int id)
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            var gameViewModel = this.Db.Games
                .Where(x => x.Id == id)
                .Select(g => new DeleteGameViewModel()
                {
                    Id = g.Id,
                    Title = g.Title
                }).FirstOrDefault();

            if (gameViewModel == null)
            {
                return this.BadRequestError("Invalid game id.");
            }

            return this.View(gameViewModel);
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Delete(IdGameInputModel model)
        {
            if (this.User.Role.ToString() != "Admin")
            {
                return this.Redirect("/");
            }

            var game = this.Db.Games.FirstOrDefault(x => x.Id == model.Id);

            if (game == null)
            {
                return this.BadRequestError("Invalid game id.");
            }

            this.Db.Games.Remove(game);
            this.Db.SaveChanges();

            return this.Redirect("/Games/All");
        }
    }
}
