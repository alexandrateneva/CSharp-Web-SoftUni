namespace ExamWebApp.Controllers
{
    using System;
    using System.Linq;
    using ExamWebApp.Models;
    using ExamWebApp.ViewModels.Packages;
    using ExamWebApp.ViewModels.Users;
    using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
    using Microsoft.EntityFrameworkCore.Query.Expressions;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class PackagesController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var package = this.Db.Packages
                .Where(x => x.Id == id)
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status.ToString(),
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.Username
                })
                .FirstOrDefault();

            if (package == null)
            {
                return this.BadRequestError("Invalid package id.");
            }

            return this.View(package);
        }

        [Authorize]
        public IHttpResponse Acquire(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid package id.");
            }

            package.Status = Status.Acquired;
            this.Db.Packages.Update(package);
            this.Db.SaveChanges();

            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            var receipt = new Receipt()
            {
                Recipient = user,
                Package = package,
                IssuedOn = DateTime.UtcNow,
                Fee = package.Weight * (decimal)2.67
            };
            this.Db.Receipts.Add(receipt);
            this.Db.SaveChanges();

            return this.Redirect("/Receipts/Index");
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = this.Db.Users
                .Select(u => new BaseUserViewModel()
                {
                    Id = u.Id,
                    Username = u.Username
                }).ToList();

            var model = new PackageCreateViewModel()
            {
                Recipients = users
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(PackageInputModel model)
        {

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 3)
            {
                var errorModel = this.GetErrorModel();
                return this.BadRequestErrorWithView("Please provide valid description with length of 3 or more characters.", errorModel);
            }

            if (string.IsNullOrWhiteSpace(model.Address) || model.Address.Trim().Length < 5)
            {
                var errorModel = this.GetErrorModel();
                return this.BadRequestErrorWithView("Please provide valid address with length of 5 or more characters.", errorModel);
            }

            if (model.Weight <= 0)
            {
                var errorModel = this.GetErrorModel();
                return this.BadRequestErrorWithView("Weight must be positive number.", errorModel);
            }

            var user = this.Db.Users.FirstOrDefault(u => u.Id == model.RecipientId);
            if (user == null)
            {
                var errorModel = this.GetErrorModel();
                return this.BadRequestErrorWithView("Please choose valid recipient from drop down menu.", errorModel);
            }

            var package = new Package()
            {
                Description = model.Description,
                Weight = model.Weight,
                ShippingAddress = model.Address,
                Status = Status.Pending,
                RecipientId = model.RecipientId
            };

            this.Db.Packages.Add(package);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            var packages = this.Db.Packages
                .Where(x => x.Status.ToString() == "Pending")
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status.ToString(),
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.Username
                }).ToList();

            var model = new ManyPackagesViewModel()
            {
               Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            var packages = this.Db.Packages
                .Where(x => x.Status.ToString() == "Shipped")
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status.ToString(),
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.Username
                }).ToList();

            var model = new ManyPackagesViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            var packages = this.Db.Packages
                .Where(x => x.Status.ToString() == "Delivered" || x.Status.ToString() == "Acquired")
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status.ToString(),
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.Username
                }).ToList();

            var model = new ManyPackagesViewModel()
            {
                Packages = packages
            };

            return this.View(model);
        }

        [Authorize("Admin")]
        public IHttpResponse Ship(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid package id.");
            }

            package.Status = Status.Shipped;

            var random = new Random();
            var days = random.Next(20, 40);

            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(days);
            this.Db.Packages.Update(package);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Deliver(int id)
        {
            var package = this.Db.Packages
                .FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid package id.");
            }

            package.Status = Status.Delivered;
            this.Db.Packages.Update(package);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        private PackageCreateViewModel GetErrorModel()
        {
            var users = this.Db.Users
                .Select(u => new BaseUserViewModel()
                {
                    Id = u.Id,
                    Username = u.Username
                }).ToList();

            var errorModel = new PackageCreateViewModel()
            {
                Recipients = users
            };

            return errorModel;
        }
    }
}
