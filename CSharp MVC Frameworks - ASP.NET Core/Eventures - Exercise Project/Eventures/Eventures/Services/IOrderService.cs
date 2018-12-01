namespace Eventures.Services
{
    using System.Collections.Generic;
    using Eventures.Models;
    using Eventures.ViewModels.Orders;
    using System.Linq;

    public interface IOrderService
    {
        Order CreateOrder(CreateOrderViewModel model);

        int GetTotalBoughtTicketsCountByEventIdFromCurrentUser(string eventId, string userId);

        IEnumerable<Order> GetAllOrdersByUserId(string id);

        IEnumerable<BaseOrderViewModel> GetAllOrders();
    }
}
