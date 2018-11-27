namespace Eventures.ViewModels.Orders
{
    using System;
    using System.Globalization;

    public class BaseOrderViewModel
    {
        public string CustomerName { get; set; }

        public string EventName { get; set; }

        public DateTime OrderedOn { get; set; }

        public string GetOrderedOnDate => this.OrderedOn.ToString("dd-MMM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
    }
}
