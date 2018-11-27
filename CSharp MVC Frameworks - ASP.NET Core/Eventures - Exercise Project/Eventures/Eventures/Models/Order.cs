namespace Eventures.Models
{
    using System;

    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string CustomerId { get; set; }
        public ApplicationUser Customer { get; set; }

        public string EventId { get; set; }
        public Event Event { get; set; }

        public DateTime OrderedOn { get; set; }

        public int TicketsCount { get; set; }
    }
}
