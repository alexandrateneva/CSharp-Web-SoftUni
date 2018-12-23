using System.Collections.Generic;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Web.Areas.Store.Models.Rentals
{
    public class AllRentalsViewModel
    {
        public AllRentalsViewModel()
        {
            this.Rentals = new HashSet<Rental>();
        }

        public ICollection<Rental> Rentals { get; set; }
    }
}
