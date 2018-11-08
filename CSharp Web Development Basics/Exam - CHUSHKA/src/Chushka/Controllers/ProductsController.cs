namespace Chushka.Controllers
{
    using System;
    using System.Linq;
    using Chushka.Models;
    using Chushka.ViewModels.Home;
    using Chushka.ViewModels.Products;
    using SIS.HTTP.Responses;
    using SIS.MvcFramework;

    public class ProductsController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var product = this.Db.Products
                .Where(x => x.Id == id)
                .Select(p => new BaseProductViewModel()
                {
                    Id = p.Id,
                    Description = p.Description,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type
                })
                .FirstOrDefault();

            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }

            return this.View(product);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var model = new ProductCreateViewModel();
            return this.View(model);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreateProductInputModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid name of product with length of 3 or more characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 20)
            {
                return this.BadRequestErrorWithView("Please provide valid description of product with length of 20 or more characters.");
            }

            if (model.Price <= 0)
            {
                return this.BadRequestErrorWithView("Price can not be zero or negative number.");
            }

            if (!Enum.TryParse(model.Type, true, out Models.Type type))
            {
                return this.BadRequestErrorWithView("Invalid channel type.");
            }

            var product = new Product()
            {
                Description = model.Description,
                Name = model.Name,
                Type = type,
                Price = model.Price
            };

            this.Db.Products.Add(product);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var product = this.Db.Products
                .Where(x => x.Id == id)
                .Select(p => new EditAndDeleteViewModel()
                {
                    Description = p.Description,
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type.ToString()
                }).FirstOrDefault();

            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }

            return this.View(product);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(EditAndDeleteViewModel model)
        {
            var product = this.Db.Products.FirstOrDefault(x => x.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }

            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                return this.BadRequestErrorWithView("Please provide valid name of product with length of 3 or more characters.");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 20)
            {
                return this.BadRequestErrorWithView("Please provide valid description of product with length of 20 or more characters.");
            }

            if (model.Price <= 0)
            {
                return this.BadRequestErrorWithView("Price can not be zero or negative number.");
            }

            if (!Enum.TryParse(model.Type, true, out Models.Type type))
            {
                return this.BadRequestErrorWithView("Invalid channel type.");
            }

            product.Description = model.Description;
            product.Name = model.Name;
            product.Type = type;
            product.Price = model.Price;

            this.Db.Products.Update(product);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var product = this.Db.Products
                .Where(x => x.Id == id)
                .Select(p => new EditAndDeleteViewModel()
                {
                    Description = p.Description,
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Type = p.Type.ToString()
                }).FirstOrDefault();

            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }

            return this.View(product);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Delete(EditAndDeleteViewModel model, string empty = null)
        {
            var product = this.Db.Products.FirstOrDefault(x => x.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }

            this.Db.Products.Remove(product);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Order(int id)
        {
            var product = this.Db.Products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return this.BadRequestError("Invalid product id.");
            }
            var user = this.Db.Users.FirstOrDefault(x => x.Username == this.User.Username);
            var order = new Order()
            {
                OrderedOn = DateTime.UtcNow,
                UserId = user.Id,
                ProductId = product.Id
            };

            this.Db.Orders.Add(order);
            this.Db.SaveChanges();

            return this.Redirect("/");
        }
    }
}
