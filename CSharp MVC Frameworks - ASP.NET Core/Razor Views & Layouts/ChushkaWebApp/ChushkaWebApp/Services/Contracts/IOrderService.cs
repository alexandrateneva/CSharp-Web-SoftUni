namespace ChushkaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.ViewModels.Orders;

    public interface IOrderService
    {
        IList<BaseOrderViewModel> GetAllOrders();

        Order Create(int productId, string userId);
    }
}
