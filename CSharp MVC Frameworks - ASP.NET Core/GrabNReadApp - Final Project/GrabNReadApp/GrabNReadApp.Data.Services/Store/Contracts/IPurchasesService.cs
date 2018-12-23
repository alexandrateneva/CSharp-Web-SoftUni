using System.Collections.Generic;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IPurchasesService
    {
        Task<Purchase> Create(Purchase purchase);

        IEnumerable<Purchase> GetAllOrderedPurchasesByOrderId(int orderId);
        
        bool Delete(int id);
    }
}
