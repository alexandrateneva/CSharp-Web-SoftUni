using System;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Data.Models.Store
{
    public class Rental
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public GrabNReadAppUser Customer { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public decimal TotalSum => this.Book.PricePerDay * (this.StartDate - this.EndDate).Days;
    }
}
