using System.Collections.Generic;

namespace GrabNReadApp.Data.Models.Books
{
    public class Genre
    {
        public Genre()
        {
            this.Books = new HashSet<Book>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public ICollection<Book> Books { get; set; }
    }
}
