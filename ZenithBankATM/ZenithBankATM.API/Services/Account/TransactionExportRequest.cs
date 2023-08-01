namespace ZenithBankATM.API.Services.Account;

public sealed record TransactionExportRequest(
    string AccountNumber,
    DateTime StartDate,
    DateTime EndDate);
