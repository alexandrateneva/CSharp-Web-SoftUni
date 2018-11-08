namespace JudgeSystem.ViewModels.Contests
{
    public class BaseContestViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int SubmissionsCount { get; set; }

        public bool HasLoggedInCreator { get; set; }
    }
}
