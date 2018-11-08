namespace Chushka.Controllers
{
    using System.Linq;
    using Chushka.ViewModels.Orders;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class OrdersController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse All()
        {
            var orders = this.Db.Orders
            .Select(p => new BaseOrderViewModel()
            {
                Id = p.Id,
                Username = p.User.Username,
                ProductName = p.Product.Name,
                OrderedOn = p.OrderedOn
            }).ToList();

            var model = new AllOrdersViewModel()
            {
                Orders = orders
            };

            return this.View(model);
        }
    }
}
