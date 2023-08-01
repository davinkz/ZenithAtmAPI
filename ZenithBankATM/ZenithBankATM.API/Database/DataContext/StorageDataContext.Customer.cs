using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

using ZenithBankATM.API.Database.Entities;

namespace ZenithBankATM.API.Database.DataContext;

public sealed partial class StorageDataContext
{
    public DbSet<Customer> Customers { get; set; }

    private static void ConfigureCustomersTable(ModelBuilder modelBuilder)
    {
        /*modelBuilder.Entity<Customer>()
            .HasMany(x => x.Accounts)
            .WithOne(x => x.Customer)
            .HasForeignKey(x => x.CustomerId)
            .HasPrincipalKey(x => x.Id);*/

        modelBuilder.Entity<Customer>()
            .Property(x => x.DateCreated)
            .ValueGeneratedOnAdd()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);

        modelBuilder.Entity<Customer>()
            .Property(x => x.DateUpdated)
            .ValueGeneratedOnUpdate()
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Save);
    }
}
