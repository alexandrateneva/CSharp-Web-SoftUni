namespace GameStore.Controllers
{
    using GameStore.Data;
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
