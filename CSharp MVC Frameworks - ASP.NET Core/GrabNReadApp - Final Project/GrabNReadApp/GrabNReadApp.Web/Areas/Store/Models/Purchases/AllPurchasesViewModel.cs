using System.Collections.Generic;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Web.Areas.Store.Models.Purchases
{
    public class AllPurchasesViewModel
    {
        public AllPurchasesViewModel()
        {
            this.Purchases = new HashSet<Purchase>();
        }

        public ICollection<Purchase> Purchases { get; set; }
    }
}
