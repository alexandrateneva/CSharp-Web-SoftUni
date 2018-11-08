namespace GameStore.Controllers
{
    using System;
    using System.Linq;
    using System.Text.RegularExpressions;
    using GameStore.Models;
    using GameStore.ViewModels.Users;
    using SIS.HTTP.Cookies;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;
    using SIS.MvcFramework.Services;

    public class UsersController : BaseController
    {
        private readonly IHashService hashService;

        public UsersController(IHashService hashService)
        {
            this.hashService = hashService;
        }

        public IHttpResponse Logout()
        {
            if (!this.Request.Cookies.ContainsCookie(".auth-cakes"))
            {
                return this.Redirect("/");
            }

            var cookie = this.Request.Cookies.GetCookie(".auth-cakes");
            cookie.Delete();
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        public IHttpResponse Login()
        {
            return this.View("Users/Login");
        }

        [HttpPost]
        public IHttpResponse Login(LoginInputModel model)
        {
            var hashedPassword = this.hashService.Hash(model.Password);

            var user = this.Db.Users.FirstOrDefault(x =>
                x.Email == model.Email.Trim() &&
                x.Password == hashedPassword);

            if (user == null)
            {
                return this.BadRequestErrorWithView("Invalid email or password.");
            }

            MvcUserInfo mvcUser = new MvcUserInfo()
            {
                Username = user.FullName,
                Role = user.Role.ToString(),
                Info = user.Email
            };

            var cookieContent = this.UserCookieService.GetUserCookie(mvcUser);

            var cookie = new HttpCookie(".auth-cakes", cookieContent, 7) { HttpOnly = true };
            this.Response.Cookies.Add(cookie);
            return this.Redirect("/");
        }

        public IHttpResponse Register()
        {
            return this.View("Users/Register");
        }

        [HttpPost]
        public IHttpResponse Register(RegisterInputModel model)
        {
            // Validate
            if (string.IsNullOrWhiteSpace(model.Email) || !model.Email.Contains('@'))
            {
                return this.BadRequestErrorWithView("Please provide valid email.");
            }

            if (string.IsNullOrWhiteSpace(model.FullName))
            {
                return this.BadRequestErrorWithView("Please provide valid full name.");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Trim().Length < 6)
            {
                return this.BadRequestErrorWithView("Please provide valid password with length of 6 or more characters.");
            }

            var regex = new Regex(@"(?=.*\d)(?=.*[a-z])(?=.*[A-Z])");
            if (!regex.IsMatch(model.Password))
            {
                return this.BadRequestErrorWithView("Password must contain at least 1 uppercase, 1 lowercase letter and 1 digit.");
            }

            if (this.Db.Users.Any(x => x.Email == model.Email.Trim()))
            {
                return this.BadRequestErrorWithView("User with the same email already exists.");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.BadRequestErrorWithView("Passwords do not match.");
            }

            // Hash password
            var hashedPassword = this.hashService.Hash(model.Password);

            var role = Role.User;
            if (!this.Db.Users.Any())
            {
                role = Role.Admin;
            }

            // Create user
            var user = new User
            {
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Password = hashedPassword,
                Role = role,
            };
            this.Db.Users.Add(user);

            try
            {
                this.Db.SaveChanges();
            }
            catch (Exception e)
            {
                // TODO: Log error
                return this.ServerError(e.Message);
            }

            // Redirect
            var loginInputModel = new LoginInputModel()
            {
                Email = model.Email.Trim(),
                Password = model.Password
            };

            return this.Login(loginInputModel);
        }
    }
}
