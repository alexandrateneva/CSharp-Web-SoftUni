using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IOrdersService
    {
        Order GetCurrentOrderByUserIdWithPurchasesAndRentals(string id);

        Order GetOrderById(int id);

        Task<Order> Create(Order order);

        Task EmptyCurrentOrder(int orderId);

        Task<Order> Update(Order order);
    }
}
