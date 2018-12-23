using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IOrdersService
    {
        Order GetCurrentOrderByUserIdWithPurchasesAndRentals(string id);

        Task<Order> Create(Order order);

        Task EmptyCurrentUserOrder(int orderId, Order order);

        IEnumerable<Order> GetAllFinishedOrders();

        Order GetOrderByIdWithPurchasesAndRentals(int id);

        bool Delete(int id);
    }
}
