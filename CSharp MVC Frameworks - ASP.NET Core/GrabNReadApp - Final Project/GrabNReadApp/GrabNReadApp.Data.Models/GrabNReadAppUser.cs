using System.Collections.Generic;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Models.Store;
using Microsoft.AspNetCore.Identity;

namespace GrabNReadApp.Data.Models
{
    // Add profile data for application users by adding properties to the GrabNReadAppUser class
    public class GrabNReadAppUser : IdentityUser
    {
        public GrabNReadAppUser()
        {
            this.Purchases = new HashSet<Purchase>();
            this.Rentals = new HashSet<Rental>();
            this.Comments = new HashSet<Comment>();
            this.Votes = new HashSet<Vote>();
            this.Articles = new HashSet<Article>();
        }

        [PersonalData]
        public string FirstName { get; set; }

        [PersonalData]
        public string LastName { get; set; }

        public int? OrderId { get; set; }
        public Order Order { get; set; }

        public ICollection<Purchase> Purchases { get; set; }

        public ICollection<Rental> Rentals { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public ICollection<Vote> Votes { get; set; }

        public ICollection<Article> Articles { get; set; }
   }
}
