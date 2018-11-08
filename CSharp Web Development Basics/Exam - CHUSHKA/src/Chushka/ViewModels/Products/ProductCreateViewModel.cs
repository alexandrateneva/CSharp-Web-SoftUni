namespace Chushka.ViewModels.Products
{
    using System;
    using System.Collections.Generic;

    public class ProductCreateViewModel
    {
        public ProductCreateViewModel()
        {
            this.Types = Enum.GetNames(typeof(Models.Type));
        }

        public ICollection<string> Types { get; set; }
    }
}
