namespace PandaWebApp.Controllers
{
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PandaWebApp.Data;
    using PandaWebApp.Models;
    using PandaWebApp.ViewModels.Home;
    using PandaWebApp.ViewModels.Packages;

    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ApplicationDbContext context;

        public HomeController(
            SignInManager<ApplicationUser> signInManager,
            ApplicationDbContext context)
        {
            this._signInManager = signInManager;
            this.context = context;
        }

        public IActionResult Index()
        {
            if (this._signInManager.IsSignedIn(this.User))
            {
                var user = this.context.Users.FirstOrDefault(u => u.UserName == this.User.Identity.Name);
                if (user == null)
                {
                    return this.Redirect("/Users/Login");
                }
                
                var model = new LoggedInPartialViewModel()
                {
                    DeliveredPackages = this.context.Packages
                        .Where(p => p.RecipientId == user.Id && p.Status == Status.Delivered)
                        .Select(x => new BasePackageViewModel()
                        {
                            Description = x.Description,
                            Id = x.Id
                        }).ToList(),

                    PendingPackages = this.context.Packages
                        .Where(p => p.RecipientId == user.Id && p.Status == Status.Pending)
                        .Select(x => new BasePackageViewModel()
                        {
                            Description = x.Description,
                            Id = x.Id
                        }).ToList(),

                    ShippedPackages = this.context.Packages
                        .Where(p => p.RecipientId == user.Id && p.Status == Status.Shipped)
                        .Select(x => new BasePackageViewModel()
                        {
                            Description = x.Description,
                            Id = x.Id
                        }).ToList()
                };
                
                return this.View(model);
            }

            return this.View();
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
