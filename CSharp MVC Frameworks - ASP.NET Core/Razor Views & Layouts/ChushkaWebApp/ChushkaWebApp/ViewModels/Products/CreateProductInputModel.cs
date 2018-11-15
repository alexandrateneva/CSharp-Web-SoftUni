namespace ChushkaWebApp.ViewModels.Products
{
    using ChushkaWebApp.Models;

    public class CreateProductInputModel
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }
    }
}
