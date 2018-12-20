using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Services.Store.Contracts
{
    public interface IRentalsServices
    {
        Task<Rental> Create(Rental rental);
    }
}
