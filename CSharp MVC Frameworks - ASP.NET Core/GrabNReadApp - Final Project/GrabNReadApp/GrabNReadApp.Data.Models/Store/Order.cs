using System;
using System.Collections.Generic;
using System.Linq;

namespace GrabNReadApp.Data.Models.Store
{
    public class Order
    {
        public Order()
        {
            this.Purchases = new HashSet<Purchase>();
            this.Rentals = new HashSet<Rental>();
        }

        public int Id { get; set; }
        
        public string CustomerId { get; set; }
        public GrabNReadAppUser Customer { get; set; }

        public DateTime OrderedOn { get; set; }

        public string Address { get; set; }

        public string RecipientName { get; set; }

        public string Phone { get; set; }

        public decimal Delivery { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Rental> Rentals { get; set; }

        public bool IsFinished { get; set; }

        public decimal TotalSum
        {
            get
            {
                var totalSum = this.Purchases.Sum(order => order.TotalSum);

                totalSum += this.Rentals.Sum(hire => hire.TotalSum);

                return totalSum;
            }
        }
    }
}
