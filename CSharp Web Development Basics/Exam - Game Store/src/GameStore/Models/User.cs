namespace GameStore.Models
{
    using System.Collections.Generic;

    public class User
    {
        public User()
        {
            this.Games = new HashSet<UserGame>();
        }

        public int Id { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        public ICollection<UserGame> Games { get; set; }
    }
}
