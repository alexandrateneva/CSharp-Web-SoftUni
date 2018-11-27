namespace Eventures.Services
{
    using System.Collections.Generic;
    using Eventures.Models;
    using Eventures.ViewModels.Orders;

    public interface IOrderService
    {
        Order CreateOrder(CreateOrderViewModel model);

        int GetTotalBoughtTicketsCountByEventIdFromCurrentUser(string eventId, string userId);

        IList<BaseOrderViewModel> GetAllOrders();
    }
}
