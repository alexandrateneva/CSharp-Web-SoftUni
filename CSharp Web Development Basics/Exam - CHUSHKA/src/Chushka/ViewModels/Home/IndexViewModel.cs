namespace Chushka.ViewModels.Home
{
    using System.Collections.Generic;
    using Chushka.ViewModels.Products;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Products = new HashSet<BaseProductViewModel>();
        }

        public ICollection<BaseProductViewModel> Products { get; set; }
    }
}
