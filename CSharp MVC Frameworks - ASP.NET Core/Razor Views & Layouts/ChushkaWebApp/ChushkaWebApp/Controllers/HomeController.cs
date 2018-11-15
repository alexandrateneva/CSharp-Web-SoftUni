namespace ChushkaWebApp.Controllers
{
    using System.Diagnostics;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Services.Contracts;
    using ChushkaWebApp.ViewModels.Home;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IProductService productService;

        public HomeController(SignInManager<ApplicationUser> signInManager, IProductService productService)
        {
            this._signInManager = signInManager;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            if (this._signInManager.IsSignedIn(this.User))
            {

                var products = this.productService.GetAllProducts();

                var model = new IndexViewModel()
                {
                    Products = products
                };

                return this.View("LoggedInIndex", model);
            }

            return this.View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

