namespace ChushkaWebApp.ViewModels.Products
{
    using System;
    using System.Collections.Generic;
    using Type = ChushkaWebApp.Models.Type;

    public class EditAndDeleteViewModel 
    {
        public EditAndDeleteViewModel()
        {
            this.Types = Enum.GetNames(typeof(Type));
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public ICollection<string> Types { get; set; }
    }
}
