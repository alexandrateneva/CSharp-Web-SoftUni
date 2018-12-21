using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data.Services.Store
{
   public class RentalsService : IRentalsServices
    {
        private readonly IRepository<Rental> rentalRepository;

        public RentalsService(IRepository<Rental> rentalRepository)
        {
            this.rentalRepository = rentalRepository;
        }

        public async Task<Rental> Create(Rental rental)
        {
            await this.rentalRepository.AddAsync(rental);
            await this.rentalRepository.SaveChangesAsync();

            return rental;
        }

        public IEnumerable<Rental> GetAllNotOrderedRentalsByOrderId(int orderId)
        {
            var rentals = this.rentalRepository
                .All()
                .Where(r => r.IsOrdered == false && r.OrderId == orderId)
                .Include(r => r.Book)
                .ToList();

            return rentals;
        }

        public async Task<Rental> MakeRentalOrdered(Rental rental)
        {
            rental.IsOrdered = true;
            this.rentalRepository.Update(rental);
            await this.rentalRepository.SaveChangesAsync();

            return rental;
        }

        public bool Delete(int id)
        {
            var rental = this.rentalRepository.All().FirstOrDefault(g => g.Id == id);
            if (rental != null)
            {
                this.rentalRepository.Delete(rental);
                this.rentalRepository.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
