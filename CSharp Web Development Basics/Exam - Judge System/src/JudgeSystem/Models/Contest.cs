namespace JudgeSystem.Models
{
    using System.Collections.Generic;

    public class Contest
    {
        public Contest()
        {
            this.Submissions = new HashSet<Submission>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public int CreatorId { get; set; }
        public User Creator { get; set; }

        public ICollection<Submission> Submissions { get; set; }
    }
}
