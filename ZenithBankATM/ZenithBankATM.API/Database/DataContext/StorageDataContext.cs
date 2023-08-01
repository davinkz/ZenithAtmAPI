using Microsoft.EntityFrameworkCore;

namespace ZenithBankATM.API.Database.DataContext;

public sealed partial class StorageDataContext : DbContext
{
    public StorageDataContext(DbContextOptions<StorageDataContext> options)
        : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCustomersTable(modelBuilder);
        ConfigureAccountsTable(modelBuilder);
        ConfigureTransactionsTable(modelBuilder);
    }
}
