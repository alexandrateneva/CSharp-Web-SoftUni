namespace PandaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using PandaWebApp.Models;
    using PandaWebApp.ViewModels.Packages;

    public interface IPackageService
    {
        Package CreatePackage(PackageCreateViewModel model);

        Package GetPackageById(int id);

        PackageDetailsViewModel GetPackageDetailsById(int id);

        IList<PackageDetailsViewModel> GetPackagesByStatus(Status status);

        IList<PackageDetailsViewModel> GetPackagesByStatus(Status status1, Status status2);

        Package AcquirePackage(Package package);

        Package ShipPackage(Package package);

        Package DeliverPackage(Package package);
    }
}
