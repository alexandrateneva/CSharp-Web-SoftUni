namespace ExamWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using ExamWebApp.ViewModels.Packages;

    public class LoggedInIndexViewModel
    {
        public LoggedInIndexViewModel()
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
