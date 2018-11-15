namespace ChushkaWebApp.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ChushkaWebApp.Data;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Services.Contracts;
    using ChushkaWebApp.ViewModels.Products;
    using Type = ChushkaWebApp.Models.Type;

    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext context;

        public ProductService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IList<BaseProductViewModel> GetAllProducts()
        {
            var products = this.context.Products.Select(p => new BaseProductViewModel()
            {
                Id = p.Id,
                Description = p.Description,
                Name = p.Name,
                Price = p.Price,
                Type = p.Type
            }).ToList();

            return products;
        }

        public Product CreateProduct(CreateProductInputModel model, Type type)
        {
            var product = new Product()
            {
                Description = model.Description,
                Name = model.Name,
                Type = type,
                Price = (decimal)model.Price
            };

            this.context.Products.Add(product);
            this.context.SaveChanges();

            return product;
        }

        public BaseProductViewModel GetBaseProductViewById(int id)
        {
            var product = this.context.Products
                .Where(x => x.Id == id)
                .Select(p => new BaseProductViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type
                }).FirstOrDefault();

            return product;
        }

        public EditAndDeleteViewModel GetProductByIdEditAndDelete(int id)
        {
            var product = this.context.Products
                .Where(x => x.Id == id)
                .Select(p => new EditAndDeleteViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = Enum.GetName(typeof(Type), p.Type)
                }).FirstOrDefault();

            return product;
        }

        public Product GetProductById(int id)
        {
            var product = this.context.Products.FirstOrDefault(x => x.Id == id);
            return product;
        }

        public Product EditProduct(Product product, EditAndDeleteViewModel model)
        {
            product.Description = model.Description;
            product.Name = model.Name;
            product.Type = (Type)Enum.Parse(typeof(Type), model.Type) ;
            product.Price = (decimal)model.Price;

            this.context.Products.Update(product);
            this.context.SaveChanges();

            return product;
        }

        public void DeleteProduct(Product product)
        {
            this.context.Products.Remove(product);
            this.context.SaveChanges();
        }
    }
}
