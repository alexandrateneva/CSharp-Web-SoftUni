using System;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Data.Models.Store
{
    public class Purchase
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }
        public GrabNReadAppUser Customer { get; set; }

        public int BookId { get; set; }
        public Book Book { get; set; }

        public int BookCount { get; set; }

        public decimal TotalSum => this.Book.Price * this.BookCount;

        public bool IsOrdered { get; set; }
    }
}
