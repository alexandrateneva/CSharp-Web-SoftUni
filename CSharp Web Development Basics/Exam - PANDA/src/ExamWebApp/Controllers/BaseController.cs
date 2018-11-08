namespace ExamWebApp.Controllers
{
    using ExamWebApp.Data;
    using SIS.MvcFramework;

    public class BaseController : Controller
    {
        public BaseController()
        {
            this.Db = new ApplicationDbContext();
        }

        protected ApplicationDbContext Db { get; }
    }
}
