namespace ChushkaWebApp.Controllers
{
    using System;
    using ChushkaWebApp.Models;
    using ChushkaWebApp.Services.Contracts;
    using ChushkaWebApp.ViewModels.Products;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    public class ProductsController : Controller
    {
        private readonly IProductService productService;

        public ProductsController(IProductService productService)
        {
            this.productService = productService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new ProductCreateViewModel();
            return this.View(model);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Create(CreateProductInputModel model)
        {
            var errorModel = new ProductCreateViewModel();

            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Name", "Please provide valid name of product with length of 3 or more characters.");
                return this.View(errorModel);
            }

            if (model.Price == null)
            {
                this.ModelState.AddModelError("Price", "Price can not be zero or negative number.");
                return this.View(errorModel);
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 20)
            {
                this.ModelState.AddModelError("Description", "Please provide valid description of product with length of 20 or more characters.");
                return this.View(errorModel);
            }

            if (!Enum.TryParse(model.Type, true, out Models.Type type))
            {
                this.ModelState.AddModelError("Type", "Invalid channel type.");
                return this.View(errorModel);
            }

            this.productService.CreateProduct(model, type);

            return this.Redirect("/");
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var product = this.productService.GetBaseProductViewById(id);
                
            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            return this.View(product);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = this.productService.GetProductByIdEditAndDelete(id);

            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            return this.View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Edit(EditAndDeleteViewModel model)
        {
            var product = this.productService.GetProductById(model.Id);

            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            var errorModel = this.productService.GetProductByIdEditAndDelete(model.Id);

            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Name", "Please provide valid name of product with length of 3 or more characters.");
                return this.View(errorModel);
            }

            if (model.Price == null)
            {
                this.ModelState.AddModelError("Price", "Price can not be zero or negative number.");
                return this.View(errorModel);
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Trim().Length < 20)
            {
                this.ModelState.AddModelError("Description", "Please provide valid description of product with length of 20 or more characters.");
                return this.View(errorModel);
            }

            if (!Enum.TryParse(model.Type, true, out Models.Type type))
            {
                this.ModelState.AddModelError("Type", "Invalid channel type.");
                return this.View(errorModel);
            }

            this.productService.EditProduct(product, model);
            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = this.productService.GetProductByIdEditAndDelete(id);

            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            return this.View(product);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult Delete(EditAndDeleteViewModel model)
        {
            var product = this.productService.GetProductById(model.Id);

            if (product == null)
            {
                this.ViewData["message"] = "Invalid product id.";
                return this.View("SimpleError");
            }

            this.productService.DeleteProduct(product);
            return this.Redirect("/");
        }
    }
}