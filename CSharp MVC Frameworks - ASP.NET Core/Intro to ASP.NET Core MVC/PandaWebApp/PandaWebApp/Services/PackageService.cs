namespace PandaWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PandaWebApp.Data;
    using PandaWebApp.Models;
    using PandaWebApp.Services.Contracts;
    using PandaWebApp.ViewModels.Packages;

    public class PackageService : IPackageService
    {
        private readonly ApplicationDbContext context;

        public PackageService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Package CreatePackage(PackageCreateViewModel model)
        {
            var package = new Package()
            {
                Description = model.Description,
                Weight = (decimal)model.Weight,
                ShippingAddress = model.Address,
                Status = Status.Pending,
                RecipientId = model.RecipientId
            };

            this.context.Packages.Add(package);
            this.context.SaveChanges();

            return package;
        }

        public Package GetPackageById(int id)
        {
            var package = this.context.Packages.FirstOrDefault(x => x.Id == id);
            return package;
        }

        public PackageDetailsViewModel GetPackageDetailsById(int id)
        {
            var package = this.context.Packages
                .Where(x => x.Id == id)
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.UserName
                })
                .FirstOrDefault();

            return package;
        }

        public IList<PackageDetailsViewModel> GetPackagesByStatus(Status status)
        {
            var packages = this.context.Packages
                .Where(x => x.Status == status)
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.UserName
                }).ToList();

            return packages;
        }

        public IList<PackageDetailsViewModel> GetPackagesByStatus(Status status1, Status status2)
        {
            var packages = this.context.Packages
                .Where(x => x.Status == status1 || x.Status == status2)
                .Select(p => new PackageDetailsViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Status = p.Status,
                    Weight = p.Weight,
                    ShippingAddress = p.ShippingAddress,
                    EstimatedDeliveryDate = p.EstimatedDeliveryDate,
                    RecipientName = p.Recipient.UserName
                }).ToList();

            return packages;
        }

        public Package AcquirePackage(Package package)
        {
            package.Status = Status.Acquired;
            this.context.Packages.Update(package);
            this.context.SaveChanges();

            return package;
        }

        public Package ShipPackage(Package package)
        {
            package.Status = Status.Shipped;

            var random = new Random();
            var days = random.Next(20, 40);

            package.EstimatedDeliveryDate = DateTime.UtcNow.AddDays(days);
            this.context.Packages.Update(package);
            this.context.SaveChanges();

            return package;
        }
        
        public Package DeliverPackage(Package package)
        {
            package.Status = Status.Delivered;
            this.context.Packages.Update(package);
            this.context.SaveChanges();

            return package;
        }
    }
}
