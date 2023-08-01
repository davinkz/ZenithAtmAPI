using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Models;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ZenithBankATM.API.Services.Account;

public interface IAccountService
{
    /// <summary>
    /// Verifies if the given account number exists.
    /// </summary>
    /// <param name="accountNumber">Account number to validate</param>
    /// <returns>bool</returns>
    Task<bool> AccountNumberExists(string accountNumber);

    /// <summary>
    /// Returns account information.
    /// </summary>
    /// <param name="accountNumber">The account number to retrieve its details.</param>
    /// <returns>Account</returns>
    Task<Database.Entities.Account?> GetAccount(string accountNumber);

    /// <summary>
    /// Creates a new customer lodgment and increases the customer's balance
    /// </summary>
    /// <param name="request">Specifies the lodgment request object.</param>
    /// <param name="cancellationToken">Propagates notification that operation should be canceled.</param>
    /// <returns>TransactionResponse</returns>
    Task<TransactionResponse> MakeDeposit(
        TransactionRequest request,
        CancellationToken cancellationToken);

    /// <summary>
    /// Creates a new customer withdrawal and decreases the customer's balance
    /// </summary>
    /// <param name="request">Specifies the withdrawal request object.</param>
    /// <param name="cancellationToken">Propagates notification that operation should be canceled.</param>
    /// <returns>TransactionResponse</returns>
    Task<TransactionResponse> MakeWithdrawal(
        TransactionRequest request,
        CancellationToken cancellationToken);

    Task<AccountBalanceResponse> GetBalance(string accountNumber);

    Task<List<Transaction>> GetTransactions(
        string accountNumber,
        DateTime startDate,
        DateTime endDate);

    Task<string> ExportTransactionsToExcel(
        Database.Entities.Account account,
        List<Transaction> transactions,
        IHostingEnvironment environment,
        HttpRequest httpRequest);
}
