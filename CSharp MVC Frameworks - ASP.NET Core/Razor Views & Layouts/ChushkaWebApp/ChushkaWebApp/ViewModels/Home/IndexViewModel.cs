namespace ChushkaWebApp.ViewModels.Home
{
    using System.Collections.Generic;
    using ChushkaWebApp.ViewModels.Products;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Products = new HashSet<BaseProductViewModel>();
        }

        public ICollection<BaseProductViewModel> Products { get; set; }
    }
}
