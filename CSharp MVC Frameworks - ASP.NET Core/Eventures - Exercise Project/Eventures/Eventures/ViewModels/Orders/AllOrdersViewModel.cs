namespace Eventures.ViewModels.Orders
{
    using X.PagedList;

    public class AllOrdersViewModel
    {
        public virtual IPagedList<BaseOrderViewModel> Orders { get; set; }

        public int CurrentPage { get; set; } 
    }
}
