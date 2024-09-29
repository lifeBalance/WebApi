
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    // This class will be used to interact with the database, allowing us
    // to perform CRUD operations on the database. We will inject this class
    // into the controller classes that use the DB (dependency injection).
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        // The `base` is for passing the options to the base class (DbContext)
        public ApplicationDBContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        // DbSet allows us to manipulate the database table (create tables, etc).
        public DbSet<Stock> Stocks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Portfolio> Portfolios { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Portfolio>().HasKey(p => new { p.AppUserId, p.StockId });

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.AppUser)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.AppUserId);
            
            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.Stock)
                .WithMany(u => u.Portfolios)
                .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles =
            [
                new() { Name = "Admin", NormalizedName = "ADMIN" },
                new() { Name = "User", NormalizedName = "USER" }
            ];

            modelBuilder.Entity<IdentityRole>().HasData(roles);
        }
    }
}