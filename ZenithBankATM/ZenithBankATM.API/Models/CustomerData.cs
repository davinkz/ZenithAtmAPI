using ZenithBankATM.API.Database.Entities;

namespace ZenithBankATM.API.Models;

public sealed record CustomerData(
    Guid Id,
    string FirstName,
    string LastName,
    string Email,
    string BVN,
    List<Account> Accounts);
