namespace JudgeSystem.Controllers
{
    using System.Linq;
    using JudgeSystem.ViewModels.Home;
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            var user = this.Db.Users.FirstOrDefault(x => x.Email == this.User.Info);
            if (user != null)
            {
              var model = new LoggedInIndexViewModel()
              {
                  FullName = user.FullName
              };

                return this.View("Home/LoggedInIndex", model);
            }
            else
            {
                return this.View();
            }
        }
    }
}
