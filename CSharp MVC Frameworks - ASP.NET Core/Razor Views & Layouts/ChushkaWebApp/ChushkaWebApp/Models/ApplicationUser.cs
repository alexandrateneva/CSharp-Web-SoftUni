namespace ChushkaWebApp.Models
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity;

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            this.Orders = new HashSet<Order>();
        }

        public string FullName { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
