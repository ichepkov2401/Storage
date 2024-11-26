using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Storage.Data.Models.Box;
using Storage.Data.Models.Pallet;

namespace Storage.Data.Contexts;
public class StorageDbContext : DbContext
{
    private string connection;

    public StorageDbContext()
    {
        connection = "Data Source=Storage.db";
    }

    public StorageDbContext(string connection)
    {
        this.connection = connection;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(connection);
        optionsBuilder.LogTo(Console.WriteLine, new[] { RelationalEventId.CommandExecuted });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<BoxStoreModel>()
            .HasOne(x => x.Pallet)
            .WithMany(y => y.Boxes)
            .HasForeignKey(x => x.PalletId);
        modelBuilder.Entity<PalletStoreModel>()
            .Ignore(x => x.Boxes);
    }

    #region Entities

    internal DbSet<BoxStoreModel> Boxes { get; set; }

    internal DbSet<PalletStoreModel> Pallets { get; set; }

    #endregion
}
