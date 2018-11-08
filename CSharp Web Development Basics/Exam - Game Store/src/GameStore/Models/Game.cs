namespace GameStore.Models
{
    using System;
    using System.Collections.Generic;

    public class Game
    {
        public Game()
        {
            this.Users = new HashSet<UserGame>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Trailer { get; set; }

        public string Image { get; set; }

        public decimal Size { get; set; }

        public decimal Price { get; set; }

        public string Description { get; set; }

        public DateTime ReleaseDate { get; set; }

        public ICollection<UserGame> Users { get; set; }
    }
}
