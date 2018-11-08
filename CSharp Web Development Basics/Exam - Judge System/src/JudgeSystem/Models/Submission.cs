namespace JudgeSystem.Models
{
    public class Submission
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public bool IsSuccessful { get; set; }

        public int ContestId { get; set; }
        public Contest Contest { get; set; }

        public int? CreatorId { get; set; }
        public User Creator { get; set; }
    }
}
