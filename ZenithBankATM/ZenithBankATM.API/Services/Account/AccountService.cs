using ClosedXML.Excel;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using ZenithBankATM.API.Database.DataContext;
using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Exceptions;
using ZenithBankATM.API.Models;
using ZenithBankATM.API.Utils;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ZenithBankATM.API.Services.Account;

internal sealed partial class AccountService : IAccountService
{
    private readonly StorageDataContext _storageDataContext;

    public AccountService(StorageDataContext storageDataContext)
    {
        _storageDataContext = storageDataContext;
    }

    public async Task<bool> AccountNumberExists(string accountNumber) =>
        await _storageDataContext
        .Accounts
        .AnyAsync(x => x.AccountNumber == accountNumber);

    public async Task<Database.Entities.Account?> GetAccount(string accountNumber) =>
        await _storageDataContext
            .Accounts
            .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

    public async Task<TransactionResponse> MakeDeposit(
        TransactionRequest request,
        CancellationToken cancellationToken)
    {
        return await PerformDepositOrWithdrawalTransaction(
            request, false, cancellationToken);
    }

    public async Task<TransactionResponse> MakeWithdrawal(
        TransactionRequest request,
        CancellationToken cancellationToken)
    {
        return await PerformDepositOrWithdrawalTransaction(
            request, true, cancellationToken);
    }

    public async Task<AccountBalanceResponse> GetBalance(string accountNumber)
    {
        Database.Entities.Account account
            = await GetAccount(accountNumber)
                ?? throw new AccountNumberNotFoundException(accountNumber);

        return new AccountBalanceResponse(
            accountNumber,
            account.Name,
            account.Balance);
    }

    public async Task<List<Transaction>> GetTransactions(
        string accountNumber,
        DateTime startDate,
        DateTime endDate)
    {
        Database.Entities.Account account
            = await GetAccount(accountNumber)
                ?? throw new InvalidAccountNumberException(accountNumber);

        return await _storageDataContext
            .Transactions
            .AsNoTracking()
            .Where(x => x.AccountId == account.Id)
            .ToListAsync();
    }

    public async Task<string> ExportTransactionsToExcel(
        Database.Entities.Account account,
        List<Transaction> transactions,
        IHostingEnvironment environment,
        HttpRequest httpRequest)
    {
        if (transactions.Count == 0) return string.Empty;

        using var workbook = new XLWorkbook();
        var worksheet = workbook.Worksheets.Add("Sheet1");

        DateTime startDate = transactions.Min(x => x.ValueDate);
        DateTime endDate = transactions.Max(x => x.ValueDate);

        // title
        worksheet.Cell("A1").Value =
            $"{account.Name} - {account.AccountNumber} transactions between " +
            $"{startDate:D} to {endDate:D}";

        // headers
        worksheet.Cell("A3").Value = "Transaction Date";
        worksheet.Cell("B3").Value = "Value Date";
        worksheet.Cell("C3").Value = "Transaction Type";
        worksheet.Cell("D3").Value = "Reference";
        worksheet.Cell("E3").Value = "Narration";
        worksheet.Cell("F3").Value = "Debit";
        worksheet.Cell("G3").Value = "Credit";
        worksheet.Cell("h3").Value = "Balance";

        // details
        decimal balance = 0;
        int sn = 4;
        foreach (Transaction transaction in transactions)
        {
            balance += transaction.Amount;
            bool isDeposit = ((TransactionType)transaction.TransactionType)
                == TransactionType.Deposit;
            worksheet.Cell($"A{sn}").Value = transaction.DateCaptured.ToString("F");
            worksheet.Cell($"B{sn}").Value = transaction.ValueDate.ToString("D");
            worksheet.Cell($"C{sn}").Value = isDeposit ? "Deposit" : "Withdrawal";
            worksheet.Cell($"D{sn}").Value = transaction.Reference;
            worksheet.Cell($"E{sn}").Value = transaction.Narration;
            worksheet.Cell($"F{sn}").Value = isDeposit ? transaction.Amount.ToString("n2") : "0.00";
            worksheet.Cell($"G{sn}").Value = !isDeposit ? (transaction.Amount * -1).ToString("n2") : "0.00";
            worksheet.Cell($"h{sn}").Value = balance.ToString("n2");

            sn++;
        }

        string fileName = Guid.NewGuid().ToString().Replace("-", "") + ".xlsx";
        string path = Path.Combine(
            environment.ContentRootPath,
            "Exports",
            fileName);

        workbook.SaveAs(path);

        string url = $"{httpRequest.Scheme}://{httpRequest.Host.Value}/Exports/{fileName}";
        return url;
    }

    private async Task<TransactionResponse> PerformDepositOrWithdrawalTransaction(
        TransactionRequest request,
        bool isWithrawal,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(request, nameof(request));

        Database.Entities.Account account
            = await GetAccount(request.AccountNumber)
                ?? throw new InvalidAccountNumberException(request.AccountNumber);

        if (request.Amount <= 0)
            throw new ApiException(
                StatusCodes.Status400BadRequest,
                "Amount must be more than zero.");

        string transactionReference = StringUtils.GenerateRandomDigits(10);
        string procedureName = isWithrawal ? "sp_MakeWithdrawal" : "sp_MakeDeposit";
        string sql = $"Execute {procedureName} @AccountId, @Amount, @Reference, @ValueDate";
        await _storageDataContext
            .Database
            .ExecuteSqlRawAsync(
                sql,
                new SqlParameter("@AccountId", account.Id),
                new SqlParameter("@Amount", request.Amount),
                new SqlParameter("@ValueDate", request.ValueDate),
                new SqlParameter("@Reference", transactionReference));

        var transaction = await _storageDataContext
            .Transactions
            .FirstOrDefaultAsync(x => x.Reference == transactionReference);

        return new TransactionResponse(
            account!.AccountNumber,
            account.Name,
            transactionReference);
    }

}
