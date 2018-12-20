using System.Collections.Generic;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Web.Areas.Store.Models.Orders
{
    public class OrderBaseViewModel
    {
        public int Id { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Rental> Rentals { get; set; }

        public decimal TotalSum { get; set; }
    }
}
