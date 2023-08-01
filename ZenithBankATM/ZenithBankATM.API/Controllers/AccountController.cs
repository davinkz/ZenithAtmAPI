using Microsoft.AspNetCore.Mvc;

using ZenithBankATM.API.Models;
using ZenithBankATM.API.Services.Account;

using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace ZenithBankATM.API.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IAccountService _accountService;
    private readonly IHostingEnvironment _environment;

    public AccountController(
        ILogger<AccountController> logger,
        IAccountService accountService,
        IHostingEnvironment environment)
    {
        _logger = logger;
        _accountService = accountService;
        _environment = environment;
    }

    [HttpPut, Route("Make-Deposit")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MakeDeposit(
        TransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        TransactionResponse transaction =
            await _accountService.MakeDeposit(request, cancellationToken);

        var response = new ApiResponse<TransactionResponse>(false, "Lodgment was successful", transaction);
        return Ok(response);
    }

    [HttpPut, Route("Make-Withdrawal")]
    [ProducesResponseType(typeof(ApiResponse<TransactionResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> MakeWithdrawal(
        TransactionRequest request,
        CancellationToken cancellationToken = default)
    {
        TransactionResponse transaction =
            await _accountService.MakeWithdrawal(request, cancellationToken);

        var response = new ApiResponse<TransactionResponse>(false, "Withdrawal was successful", transaction);
        return Ok(response);
    }

    /// <summary>
    /// Returns customer's account balance
    /// </summary>
    /// <param name="accountNumber">Account number to retrieve its balance</param>
    /// <returns>AccountBalanceResponse</returns>
    [HttpGet, Route("Balance/{accountNumber}")]
    [ProducesResponseType(typeof(ApiResponse<AccountBalanceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetBalance(string accountNumber)
    {
        AccountBalanceResponse accountBalance =
            await _accountService.GetBalance(accountNumber);

        var response = new ApiResponse<AccountBalanceResponse>(false, "", accountBalance);
        return Ok(response);
    }

    [HttpPost, Route("List-And-Export-Transaction")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetTransactionsAndExportToExcel(
        TransactionExportRequest request)
    {
        var account = await _accountService.GetAccount(request.AccountNumber);
        var transactions = await _accountService
            .GetTransactions(
                request.AccountNumber,
                request.StartDate,
                request.EndDate);

        string exportUrl = await _accountService
            .ExportTransactionsToExcel(
                account!,
                transactions,
                _environment,
                Request);

        var response = new ApiResponse<object>(
            false,
            "Statement export was successful.",
            new
            {
                statementUrl = exportUrl
            });
        return Ok(response);
    }
}
