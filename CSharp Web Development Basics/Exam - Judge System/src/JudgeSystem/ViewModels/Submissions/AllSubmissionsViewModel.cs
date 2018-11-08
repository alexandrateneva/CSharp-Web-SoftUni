namespace JudgeSystem.ViewModels.Submissions
{
    using System.Collections.Generic;
    using JudgeSystem.ViewModels.Contests;

    public class AllSubmissionsViewModel
    {
        public AllSubmissionsViewModel()
        {
            this.Contests = new HashSet<BaseContestViewModel>();
            this.Submissions = new HashSet<BaseSubmissionViewModel>();
        }

        public ICollection<BaseContestViewModel> Contests { get; set; }

        public ICollection<BaseSubmissionViewModel> Submissions { get; set; }
    }
}
