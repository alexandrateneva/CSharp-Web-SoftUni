using GrabNReadApp.Data.Models.Store;
using X.PagedList;

namespace GrabNReadApp.Web.Areas.Store.Models.Orders
{
    public class AllOrdersViewModel
    {
        public IPagedList<Order> Orders { get; set; }

        public int CurrentPage { get; set; }
    }
}
