namespace GameStore.ViewModels.Home
{
    using System.Collections.Generic;
    using GameStore.ViewModels.Games;

    public class IndexViewModel
    {
        public ICollection<BaseGameViewModel> Games { get; set; }
    }
}
