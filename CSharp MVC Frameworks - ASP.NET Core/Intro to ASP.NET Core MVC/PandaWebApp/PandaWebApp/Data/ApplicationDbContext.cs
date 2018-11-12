using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PandaWebApp.Models;

namespace PandaWebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Package> Packages { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Package>()
                .HasOne(p => p.Receipt)
                .WithOne(i => i.Package)
                .HasForeignKey<Receipt>(b => b.PackageId);

            base.OnModelCreating(builder);
        }
    }
}
