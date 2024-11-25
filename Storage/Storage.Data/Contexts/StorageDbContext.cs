using Microsoft.EntityFrameworkCore;
using Storage.Data.Entity;

namespace Storage.Data.Contexts
{
    public class StorageDbContext : DbContext
    {
        private string connection; 
        public StorageDbContext(string connection)
        {
            this.connection = connection;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Box>();
            modelBuilder.Entity<Pallet>();
        }

        #region Entities

        public DbSet<Box> Boxes { get; set; }

        public DbSet<Pallet> Pallets { get; set; }

        #endregion
    }
}
