namespace JudgeSystem.ViewModels.Submissions
{
    using System.Collections.Generic;
    using JudgeSystem.ViewModels.Contests;

    public class CreateSubmissionViewModel
    {
        public CreateSubmissionViewModel()
        {
            this.Contests = new HashSet<BaseContestViewModel>();
        }

        public ICollection<BaseContestViewModel> Contests { get; set; }
    }
}
