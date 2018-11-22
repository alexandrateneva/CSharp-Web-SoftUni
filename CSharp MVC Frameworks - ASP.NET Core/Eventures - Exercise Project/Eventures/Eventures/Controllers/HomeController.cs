namespace Eventures.Controllers
{
    using System.Diagnostics;
    using Eventures.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        
        public HomeController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Index()
        {
            if (this.signInManager.IsSignedIn(this.User))
            {
                if (this.User.IsInRole("Admin"))
                {
                    return this.View("AdminIndex");
                }

                return this.View("LoggedInIndex");
            }

            return this.View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
