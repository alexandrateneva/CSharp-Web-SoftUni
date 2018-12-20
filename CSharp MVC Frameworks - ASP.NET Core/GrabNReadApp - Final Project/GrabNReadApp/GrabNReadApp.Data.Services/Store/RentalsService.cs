using System.Threading.Tasks;
using GrabNReadApp.Data.Contracts;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Data.Services.Store.Contracts;

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
    }
}
