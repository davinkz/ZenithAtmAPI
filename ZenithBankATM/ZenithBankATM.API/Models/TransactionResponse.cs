namespace ZenithBankATM.API.Models;

public sealed record TransactionResponse(
    string AccountNumber,
    string AccountName,
    string Reference);