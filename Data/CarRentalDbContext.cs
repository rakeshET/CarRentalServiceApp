using CarRentalApp.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace CarRentalApp.Data
{
    public class CarRentalDbContext : DbContext
    {
        public CarRentalDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rental>(entity =>
            {
                entity.Property(e => e.TotalCost)
                    .HasColumnType("numeric");
            });

        }

    }
}
