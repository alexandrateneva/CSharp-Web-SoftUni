namespace ChushkaWebApp.ViewModels.Products
{
    using ChushkaWebApp.Models;

    public class BaseProductViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public Type Type { get; set; }

        public string ShortDescription
        {
            get
            {
                if (this.Description.Length > 50)
                {
                    return this.Description.Substring(0, 50) + "...";
                }
                return this.Description;
            }
        }
    }
}
