namespace JudgeSystem.ViewModels.Contests
{
    using System.Collections.Generic;

    public class AllContestsViewModel
    {
        public AllContestsViewModel()
        {
            this.Contests = new HashSet<BaseContestViewModel>();
        }

        public ICollection<BaseContestViewModel> Contests { get; set; }
    }
}
