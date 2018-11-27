namespace Eventures.ViewModels.Orders
{
    using System.Collections.Generic;

    public class AllOrdersViewModel
    {
        public AllOrdersViewModel()
        {
            this.Orders = new HashSet<BaseOrderViewModel>();
        }

        public ICollection<BaseOrderViewModel> Orders { get; set; }
    }
}
