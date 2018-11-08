namespace ExamWebApp.Controllers
{
    using System.Linq;
    using ExamWebApp.ViewModels.Home;
    using ExamWebApp.ViewModels.Packages;
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (User.IsLoggedIn)
            {
                var user = this.Db.Users.FirstOrDefault(u => u.Username == this.User.Username);
                if (user == null)
                {
                    return this.Redirect("/Users/Login");
                }

                var model = new LoggedInIndexViewModel()
                {
                    DeliveredPackages = this.Db.Packages
                        .Where(p => p.RecipientId == user.Id && p.Status.ToString() == "Delivered")
                        .Select(x => new BasePackageViewModel()
                        {
                            Description = x.Description,
                            Id = x.Id
                        }).ToList(),

                    PendingPackages = this.Db.Packages
                    .Where(p => p.RecipientId == user.Id && p.Status.ToString() == "Pending")
                    .Select(x => new BasePackageViewModel()
                    {
                        Description = x.Description,
                        Id = x.Id
                    }).ToList(),

                    ShippedPackages = this.Db.Packages
                    .Where(p => p.RecipientId == user.Id && p.Status.ToString() == "Shipped")
                    .Select(x => new BasePackageViewModel()
                    {
                        Description = x.Description,
                        Id = x.Id
                    }).ToList()
                };


                return this.View("/Home/LoggedInIndex", model);
            }

            return this.View();
        }
    }
}
