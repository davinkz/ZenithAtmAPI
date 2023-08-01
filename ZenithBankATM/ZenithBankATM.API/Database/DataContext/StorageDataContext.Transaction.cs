using Microsoft.EntityFrameworkCore;

using ZenithBankATM.API.Database.Entities;

namespace ZenithBankATM.API.Database.DataContext;

public sealed partial class StorageDataContext
{
    public DbSet<Transaction> Transactions { get; set; }

    private static void ConfigureTransactionsTable(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
            .HasOne(x => x.Account);
    }
}
