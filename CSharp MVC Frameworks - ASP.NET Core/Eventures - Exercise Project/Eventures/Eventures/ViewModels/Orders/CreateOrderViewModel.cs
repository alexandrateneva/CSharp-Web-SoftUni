namespace Eventures.ViewModels.Orders
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class CreateOrderViewModel
    {
        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Tickets count must be positive number.")]
        public int TicketsCount { get; set; }

        [Required]
        public string EventId { get; set; }
        
        public string CustomerId { get; set; }
    }
}
