using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Purchase> GetAllNotOrderedPurchasesByOrderId(int orderId)
        {
            var purchases = this.purchaseRepository
                .All()
                .Where(p => p.IsOrdered == false && p.OrderId == orderId)
                .Include(p => p.Book)
                .ToList();

            return purchases;
        }

        public async Task<Purchase> MakePurchaseOrdered(Purchase purchase)
        {
            purchase.IsOrdered = true;
            this.purchaseRepository.Update(purchase);
            await this.purchaseRepository.SaveChangesAsync();

            return purchase;
        }

        public bool Delete(int id)
        {
            var purchase = this.purchaseRepository.All().FirstOrDefault(g => g.Id == id);
            if (purchase != null)
            {
                this.purchaseRepository.Delete(purchase);
                this.purchaseRepository.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
