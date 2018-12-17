using System;
using System.Collections.Generic;
using System.Linq;
using GrabNReadApp.Data.Models.Enums;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Models.Store;

namespace GrabNReadApp.Data.Models.Books
{
    public class Book
    {
        public Book()
        {
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string Author { get; set; }

        public DateTime ReleaseDate { get; set; }

        public decimal Price { get; set; }

        public decimal PricePerDay { get; set; }

        public int Pages { get; set; }

        public CoverType CoverType { get; set; }

        public string Description { get; set; }

        public string CoverImage { get; set; }

        public decimal Rating
        {
            get
            {
                if (this.Votes.Count > 0)
                {
                    var rating = this.Votes.Sum(order => order.VoteValue);

                    rating = rating / this.Votes.Count;

                    return rating;
                }

                return 0;
            }
        }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public ICollection<Comment> Comments{ get; set; }

        public ICollection<Vote> Votes { get; set; }
    }
}
