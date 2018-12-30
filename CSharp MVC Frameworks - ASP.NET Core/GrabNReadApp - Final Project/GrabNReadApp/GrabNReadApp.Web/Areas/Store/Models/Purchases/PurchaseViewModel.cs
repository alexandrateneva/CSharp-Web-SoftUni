using System;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models.Products;
using GrabNReadApp.Web.Constants.Store;

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
        [Range(PurchasesConstants.BooksCountMinValue, PurchasesConstants.BooksCountMaxValue, ErrorMessage = PurchasesConstants.ErrorMessageForBookCount)]
        public int BookCount { get; set; }

        public decimal TotalSum { get; set; }
    }
}
