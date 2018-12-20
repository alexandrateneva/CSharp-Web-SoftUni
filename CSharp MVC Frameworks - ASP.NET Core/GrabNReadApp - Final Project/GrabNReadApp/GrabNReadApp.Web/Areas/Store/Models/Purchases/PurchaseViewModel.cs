using System;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Web.Areas.Store.Models.Purchases
{
    public class PurchaseViewModel
    {
        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }
        
        public string CustomerId { get; set; }

        public int OrderId { get; set; }

        [Required]
        [Display(Name = "Book Count")]
        [Range(1, 20, ErrorMessage = "Уou can buy from 1 to 20 pieces of this book.")]
        public int BookCount { get; set; }

        public decimal TotalSum { get; set; }
    }
}
