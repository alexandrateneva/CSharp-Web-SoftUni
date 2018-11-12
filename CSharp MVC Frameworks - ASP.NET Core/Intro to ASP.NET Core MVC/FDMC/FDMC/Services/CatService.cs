namespace FDMC.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using FDMC.Data;
    using FDMC.Models;
    using FDMC.ViewModels.Cats;

    public class CatService : ICatService
    {
        private readonly ApplicationDbContext context;

        public CatService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Cat CreateCat(CatCreateViewModel model)
        {
            var cat = new Cat()
            {
                Name = model.Name,
                Breed = model.Breed,
                Age = model.Age,
                ImageUrl = model.ImageUrl
            };

            this.context.Cats.Add(cat);
            this.context.SaveChanges();

            return cat;
        }

        public CatDetailsViewModel GetCatDetailsById(int id)
        {
            var cat = this.context.Cats
                .Where(x => x.Id == id)
                .Select(p => new CatDetailsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Breed = p.Breed,
                    Age = p.Age,
                    ImageUrl = p.ImageUrl
                })
                .FirstOrDefault();

            return cat;
        }

        public IList<CatDetailsViewModel> GetAllCats()
        {
            var cats = this.context.Cats
                .Select(p => new CatDetailsViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Breed = p.Breed,
                    Age = p.Age,
                    ImageUrl = p.ImageUrl
                })
                .ToList();

            return cats;
        }
    }
}
