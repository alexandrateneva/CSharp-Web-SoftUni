namespace Chushka.ViewModels.Products
{
    using System;
    using System.Collections.Generic;

    public class EditAndDeleteViewModel 
    {
        public EditAndDeleteViewModel()
        {
            this.Types = Enum.GetNames(typeof(Models.Type));
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public ICollection<string> Types { get; set; }
    }
}
