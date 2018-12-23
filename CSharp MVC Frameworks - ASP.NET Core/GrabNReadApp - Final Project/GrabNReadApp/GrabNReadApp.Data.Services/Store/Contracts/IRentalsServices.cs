using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IRentalsServices
    {
        Task<Rental> Create(Rental rental);

        Task<Rental> GetRentalById(int id);

        IEnumerable<Rental> GetAllOrderedRentalsByOrderId(int orderId);

        bool Delete(int id);
    }
}
