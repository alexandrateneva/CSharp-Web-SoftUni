namespace Eventures.ViewModels.Events
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;

    public class CreateEventViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 10)]
        public string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Place { get; set; }

        [Required]
        [Display(Name = "Start Date and Time")]
        [DataType(DataType.Date)]
        public DateTime? Start { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date and Time")]
        public DateTime? End { get; set; }

        [Required]
        [Range(1, Int32.MaxValue, ErrorMessage = "Tickets count must be positive number.")]
        public int TotalTickets { get; set; } 

        [Required]
        [Range(typeof(decimal), "0.01", "79228162514264337593543950335", ErrorMessage = "Price per ticket must be positive number.")]
        public decimal PricePerTicket { get; set; }
    }
}
