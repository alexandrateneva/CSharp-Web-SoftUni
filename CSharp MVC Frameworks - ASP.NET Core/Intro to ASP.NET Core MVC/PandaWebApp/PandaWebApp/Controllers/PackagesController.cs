namespace PandaWebApp.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PandaWebApp.Models;
    using PandaWebApp.Services.Contracts;
    using PandaWebApp.ViewModels;
    using PandaWebApp.ViewModels.Packages;
    using PandaWebApp.ViewModels.Users;

    public class PackagesController : Controller
    {
        private readonly IPackageService packageService;
        private readonly IUserService userService;
        private readonly IReceiptService receiptService;

        public PackagesController(
            IPackageService packageService,
            IUserService userService,
            IReceiptService receiptService)
        {
            this.packageService = packageService;
            this.userService = userService;
            this.receiptService = receiptService;
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var package = this.packageService.GetPackageDetailsById(id);

            if (package == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Package not found or matched."
                });
            }

            return this.View(package);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var users = this.userService.GetAllUsers();

            var model = new PackageCreateViewModel()
            {
                Recipients = users
            };

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(PackageCreateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 3)
            {
                var errorModel = this.GetErrorModel();
                this.ModelState.AddModelError("Description", "Please provide valid description with length of 3 or more characters.");
                return this.View(errorModel);
            }

            if (string.IsNullOrWhiteSpace(model.Address) || model.Address.Trim().Length < 5)
            {
                var errorModel = this.GetErrorModel();
                this.ModelState.AddModelError("Address", "Please provide valid address with length of 5 or more characters.");
                return this.View(errorModel);
            }

            if (model.Weight == null)
            {
                var errorModel = this.GetErrorModel();
                this.ModelState.AddModelError("Weight", "Please provide valid positive weight.");
                return this.View(errorModel);
            }

            var user = this.userService.GetUserById(model.RecipientId);
            if (user == null)
            {
                var errorModel = this.GetErrorModel();
                this.ModelState.AddModelError("RecipientId", "Please choose valid recipient from drop down menu.");
                return this.View(errorModel);
            }

            this.packageService.CreatePackage(model);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Acquire(int id)
        {
            var package = this.packageService.GetPackageById(id);

            if (package == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Package not found or matched."
                });
            }

            this.packageService.AcquirePackage(package);

            var user = this.userService.GetUserByUsername(this.User.Identity.Name);

            this.receiptService.CreateReceipt(package, user);

            return this.Redirect("/Receipts/Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Pending()
        {
            var packages = this.packageService.GetPackagesByStatus(Status.Pending);

            var model = new ManyPackagesViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Shipped()
        {
            var packages = this.packageService.GetPackagesByStatus(Status.Shipped);

            var model = new ManyPackagesViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delivered()
        {
            var packages = this.packageService.GetPackagesByStatus(Status.Delivered, Status.Acquired);

            var model = new ManyPackagesViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Ship(int id)
        {
            var package = this.packageService.GetPackageById(id);

            if (package == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Package not found or matched."
                });
            }

            this.packageService.ShipPackage(package);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Deliver(int id)
        {
            var package = this.packageService.GetPackageById(id);

            if (package == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Package not found or matched."
                });
            }

            this.packageService.DeliverPackage(package);

            return this.Redirect("/");
        }

        private PackageCreateViewModel GetErrorModel()
        {
            var users = this.userService.GetAllUsers();

            var errorModel = new PackageCreateViewModel()
            {
                Recipients = users
            };

            return errorModel;
        }
    }
}