using GrabNReadApp.Data.Models;
using GrabNReadApp.Data.Models.Blog;
using GrabNReadApp.Data.Models.Books;
using GrabNReadApp.Data.Models.Evaluation;
using GrabNReadApp.Data.Models.Store;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GrabNReadApp.Data
{
    public class GrabNReadAppContext : IdentityDbContext<GrabNReadAppUser>
    {
        public GrabNReadAppContext(DbContextOptions<GrabNReadAppContext> options)
            : base(options)
        {
        }

        public DbSet<Article> Articles { get; set; }

        public DbSet<Book> Books { get; set; }

        public DbSet<Genre> Genre { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Vote> Votes { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Purchase> Purchases { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Order>()
                .HasOne(p => p.Customer)
                .WithOne(i => i.Order)
                .HasForeignKey<GrabNReadAppUser>(b => b.OrderId);

            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
