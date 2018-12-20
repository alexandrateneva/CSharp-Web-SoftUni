using System;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models.Products;

namespace GrabNReadApp.Web.Areas.Store.Models.Rentals
{
    public class RentalViewModel
    {
        [Required]
        public int BookId { get; set; }

        public Book Book { get; set; }

        public string CustomerId { get; set; }

        public int OrderId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        public decimal TotalSum { get; set; }
    }
}
