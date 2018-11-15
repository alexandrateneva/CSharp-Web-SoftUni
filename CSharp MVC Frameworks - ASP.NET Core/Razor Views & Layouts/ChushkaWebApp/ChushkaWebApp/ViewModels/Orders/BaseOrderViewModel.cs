namespace ChushkaWebApp.ViewModels.Orders
{
    using System;
    using System.Globalization;

    public class BaseOrderViewModel
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public string ProductName { get; set; }

        public DateTime OrderedOn { get; set; }

        public string OrderedOnDate => this.OrderedOn.ToLocalTime().ToString("HH:mm dd/MM/yyyy", CultureInfo.InvariantCulture);
    }
}
