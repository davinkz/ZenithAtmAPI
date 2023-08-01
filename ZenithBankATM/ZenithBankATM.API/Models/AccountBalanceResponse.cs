namespace ZenithBankATM.API.Models;

public sealed record AccountBalanceResponse(
    string AccountNumber,
    string AccountName,
    decimal Balance);