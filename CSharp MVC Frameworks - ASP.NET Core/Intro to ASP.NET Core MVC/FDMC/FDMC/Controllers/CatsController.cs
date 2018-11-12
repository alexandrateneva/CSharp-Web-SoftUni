namespace FDMC.Controllers
{
    using FDMC.Services;
    using FDMC.ViewModels;
    using FDMC.ViewModels.Cats;
    using Microsoft.AspNetCore.Mvc;

    public class CatsController : Controller
    {
        private readonly ICatService catService;

        public CatsController(ICatService catService)
        {
            this.catService = catService;
        }

       public IActionResult Details(int id)
        {
            var cat = this.catService.GetCatDetailsById(id);

            if (cat == null)
            {
                return this.View("SimpleError", new SimpleErrorViewModel()
                {
                    Message = "Cat not found or matched."
                });
            }

            return this.View(cat);
        }

       
        public IActionResult Create()
        {
            return this.View();
        }
       
        [HttpPost]
        public IActionResult Create(CatCreateViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.Name) || model.Name.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Name", "Please provide valid name with length of 3 or more characters.");
                return this.View();
            }

            if (string.IsNullOrWhiteSpace(model.Breed) || model.Breed.Trim().Length < 3)
            {
                this.ModelState.AddModelError("Breed", "Please provide valid breed with length of 3 or more characters.");
                return this.View();
            }

            if (model.Age < 0)
            {
                this.ModelState.AddModelError("Age", "Please provide valid positive age.");
            }

            if (string.IsNullOrWhiteSpace(model.ImageUrl) || (!model.ImageUrl.StartsWith("http") && !model.ImageUrl.StartsWith("https")))
            {
                this.ModelState.AddModelError("Name", "Please provide valid image url.");
                return this.View();
            }

            this.catService.CreateCat(model);

            return this.Redirect("/");
        }
    }
}
