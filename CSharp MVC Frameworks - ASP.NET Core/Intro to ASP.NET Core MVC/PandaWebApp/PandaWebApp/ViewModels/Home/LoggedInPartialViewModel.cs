namespace PandaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using PandaWebApp.ViewModels.Packages;

    public class LoggedInPartialViewModel
    {
        public LoggedInPartialViewModel()
        {
            this.PendingPackages = new HashSet<BasePackageViewModel>();
            this.ShippedPackages = new HashSet<BasePackageViewModel>();
            this.DeliveredPackages = new HashSet<BasePackageViewModel>();
        }

        public ICollection<BasePackageViewModel> PendingPackages { get; set; }

        public ICollection<BasePackageViewModel> ShippedPackages { get; set; }

        public ICollection<BasePackageViewModel> DeliveredPackages { get; set; }
    }
}
