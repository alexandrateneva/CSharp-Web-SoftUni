using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Store;
using GrabNReadApp.Web.Constants.Store;

namespace GrabNReadApp.Web.Areas.Store.Models.Orders
{
    public class OrderDetailsViewModel
    {
        public OrderDetailsViewModel()
        {
            this.Purchases = new HashSet<Purchase>();
            this.Rentals = new HashSet<Rental>();
        }

        [Required]
        public int Id { get; set; }
        
        public string CustomerId { get; set; }

        public GrabNReadAppUser Customer { get; set; }

        public DateTime OrderedOn { get; set; }

        [Required]
        [StringLength(OrdersConstants.AddressMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = OrdersConstants.AddressMinLength)]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Recipient Name")]
        [StringLength(OrdersConstants.RecipientNameMaxLength, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = OrdersConstants.RecipientNameMinLength)]
        public string RecipientName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Delivery Options")]
        [Range(OrdersConstants.DeliveryMinValue, OrdersConstants.DeliveryMaxValue)]
        public decimal Delivery { get; set; }

        public decimal TotalSum { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Rental> Rentals { get; set; }
    }
}
