namespace FDMC.Services
{
    using System.Collections.Generic;
    using FDMC.Models;
    using FDMC.ViewModels.Cats;

    public interface ICatService
    {
        Cat CreateCat(CatCreateViewModel model);

        CatDetailsViewModel GetCatDetailsById(int id);

        IList<CatDetailsViewModel> GetAllCats();
    }
}
