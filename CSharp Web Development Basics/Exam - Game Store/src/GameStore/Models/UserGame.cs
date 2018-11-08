﻿namespace GameStore.Models
{
    public class UserGame
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public int Game { get; set; }
    }
}
