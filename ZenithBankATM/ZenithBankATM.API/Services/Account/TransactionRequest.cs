using System.ComponentModel.DataAnnotations;

namespace ZenithBankATM.API.Services.Account;

public record TransactionRequest(
    /// <summary>
    /// A valid customer account number to lodge deposit against
    /// </summary>
    [Required]
    string AccountNumber,

    /// <summary>
    /// Amount to lodge into the customer's account
    /// </summary>
    [Required]
    decimal Amount,

    /// <summary>
    /// Date of lodgement
    /// </summary>
    [Required]
    DateTime ValueDate);