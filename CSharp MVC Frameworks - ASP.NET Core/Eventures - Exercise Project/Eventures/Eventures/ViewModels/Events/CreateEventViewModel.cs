namespace Eventures.ViewModels.Events
{
    using System;

    public class CreateEventViewModel
    {
        public string Name { get; set; }

        public string Place { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public int? TotalTickets { get; set; }

        public decimal? PricePerTicket { get; set; }
    }
}
