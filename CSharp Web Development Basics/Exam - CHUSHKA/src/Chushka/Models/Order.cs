namespace Chushka.Models
{
    using System;

    public class Order
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}
