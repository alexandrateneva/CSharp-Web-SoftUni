namespace ExamWebApp.Data
{
    using ExamWebApp.Models;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Receipt> Receipts { get; set; }

        public DbSet<Package> Packages { get; set; }

        public ApplicationDbContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=Panda;Integrated Security=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Package.Receipt

            modelBuilder.Entity<Package>()
                .HasOne(p => p.Receipt)
                .WithOne(i => i.Package)
                .HasForeignKey<Receipt>(b => b.PackageId);
        }
    }
}
