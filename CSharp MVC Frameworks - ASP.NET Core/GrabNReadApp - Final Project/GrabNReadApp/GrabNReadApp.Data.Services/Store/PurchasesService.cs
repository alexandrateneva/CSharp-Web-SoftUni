using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;

namespace GrabNReadApp.Data.Services.Store
{
    public class PurchasesService : IPurchasesService
    {
        private readonly IRepository<Purchase> purchaseRepository;

        public PurchasesService(IRepository<Purchase> purchaseRepository)
        {
            this.purchaseRepository = purchaseRepository;
        }

        public async Task<Purchase> Create(Purchase purchase)
        {
            await this.purchaseRepository.AddAsync(purchase);
            await this.purchaseRepository.SaveChangesAsync();

            return purchase;
        }
    }
}
