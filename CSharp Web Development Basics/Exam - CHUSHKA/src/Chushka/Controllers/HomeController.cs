namespace Chushka.Controllers
{
    using System.Linq;
    using Chushka.ViewModels.Home;
    using Chushka.ViewModels.Products;
    using SIS.HTTP.Responses;

    public class HomeController : BaseController
    {
        public IHttpResponse Index()
        {
            if (User.IsLoggedIn)
            {

                var products = this.Db.Products.Select(p => new BaseProductViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type
                }).ToList();

                var model = new IndexViewModel()
                {
                    Products = products
                };

                return this.View("/Home/LoggedInIndex", model);
            }

            return this.View();
        }
    }
}
