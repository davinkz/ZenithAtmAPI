using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;

namespace ZenithBankATM.API.Database.Entities;

public class Transaction
{
    public Guid Id { get; set; }

    [Required]
    public Guid AccountId { get; set; }

    [Required]
    public TransactionType TransactionType { get; set; }

    [Required, StringLength(30)]
    public string Reference { get; set; } = string.Empty;

    [Required]
    public DateTime DateCaptured { get; set; }

    [Required]
    public DateTime ValueDate { get; set; }

    [Precision(18, 2)]
    public decimal Amount { get; set; }

    [Required]
    public string Narration { get; set; } = string.Empty;

    public Account Account { get; set; } = null!;
}

public enum TransactionType
{
    Deposit = 1,
    Withdrawal = 2
}