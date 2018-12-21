using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IRentalsServices
    {
        Task<Rental> Create(Rental rental);

        IEnumerable<Rental> GetAllNotOrderedRentalsByOrderId(int orderId);

        Task<Rental> MakeRentalOrdered(Rental rental);

        bool Delete(int id);
    }
}
