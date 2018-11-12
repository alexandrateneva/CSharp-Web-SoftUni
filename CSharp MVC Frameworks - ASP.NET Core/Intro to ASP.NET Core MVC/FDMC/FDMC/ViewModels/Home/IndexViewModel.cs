namespace FDMC.ViewModels.Home
{
    using System.Collections.Generic;
    using FDMC.ViewModels.Cats;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Cats = new List<CatDetailsViewModel>();
        }

        public IList<CatDetailsViewModel> Cats { get; set; }
    }
}
