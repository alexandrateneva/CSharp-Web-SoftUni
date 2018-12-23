using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Web.Areas.Store.Models.Orders
{
    public class AllOrdersViewModel
    {
        public AllOrdersViewModel()
        {
            this.Orders = new HashSet<Order>();
        }

        public ICollection<Order> Orders { get; set; }
    }
}
