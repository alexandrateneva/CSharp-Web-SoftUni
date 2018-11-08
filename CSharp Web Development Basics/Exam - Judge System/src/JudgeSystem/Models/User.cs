namespace JudgeSystem.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Contests = new HashSet<Contest>();
            this.Submissions = new HashSet<Submission>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<Contest> Contests { get; set; }

        public ICollection<Submission> Submissions { get; set; }
    }
}
