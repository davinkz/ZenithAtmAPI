using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using ZenithBankATM.API.Database.Entities;

namespace ZenithBankATM.API.Database.DataContext;

public sealed partial class StorageDataContext
{
    public DbSet<Account> Accounts { get; set; }

    private void ConfigureAccountsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(x => x.Customer);

        modelBuilder.Entity<Account>()
            .Property(x => x.DateCreated)
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

        modelBuilder.Entity<Account>()
            .Property(x => x.DateUpdated)
            .ValueGeneratedOnUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

        modelBuilder.Entity<Account>()
            .HasMany(x => x.Transactions);
    }
}
