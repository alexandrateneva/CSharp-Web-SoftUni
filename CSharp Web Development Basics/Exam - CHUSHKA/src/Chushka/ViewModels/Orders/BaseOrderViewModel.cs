namespace Chushka.ViewModels.Orders
{
    using System;

    public class BaseOrderViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string ProductName { get; set; }

        public DateTime OrderedOn { get; set; }
    }
}
