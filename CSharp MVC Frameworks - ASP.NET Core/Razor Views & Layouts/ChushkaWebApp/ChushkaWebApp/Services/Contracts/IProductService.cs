namespace ChushkaWebApp.Services.Contracts
{
    using System.Collections.Generic;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.ViewModels.Products;

    public interface IProductService
    {
        IList<BaseProductViewModel> GetAllProducts();

        Product CreateProduct(CreateProductInputModel model, Type type);

        Product GetProductById(int id);

        BaseProductViewModel GetBaseProductViewById(int id);

        EditAndDeleteViewModel GetProductByIdEditAndDelete(int id);

        Product EditProduct(Product product, EditAndDeleteViewModel model);

        void DeleteProduct(Product product);
    }
}
