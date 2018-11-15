namespace ChushkaWebApp.Models
{
    using System;

    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}
