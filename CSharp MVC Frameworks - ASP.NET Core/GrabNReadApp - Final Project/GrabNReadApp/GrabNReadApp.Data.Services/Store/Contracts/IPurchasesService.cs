using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IPurchasesService
    {
        Task<Purchase> Create(Purchase purchase);
    }
}
