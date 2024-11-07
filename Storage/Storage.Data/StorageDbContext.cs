using Microsoft.EntityFrameworkCore;
using Storage.Data.Entity;

namespace Storage.Data
{
    public class StorageDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=Storage.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Box>()
                .Property("expirationDate")
                .IsRequired(false)
                .HasField("expirationDate")
                .HasColumnName("ExpirationDate");

            modelBuilder.Entity<Pallet>();
        }

        #region Entities

        public DbSet<Box> Boxes { get; set; }

        public DbSet<Pallet> Pallets { get; set; }

        #endregion
    }
}
