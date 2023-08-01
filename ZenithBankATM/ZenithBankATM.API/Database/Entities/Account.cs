using Microsoft.EntityFrameworkCore;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithBankATM.API.Database.Entities;

public class Account
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(10)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    public AccountType AccountType { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public bool HasATMCard { get; set; } = false;

    public CardType ATMCardType { get; set; } = CardType.NA;

    [StringLength(20)]
    public string ATMCardNumber { get; set; } = string.Empty;

    [StringLength(3)]
    public string ATMCardCCV { get; set; } = string.Empty;

    [StringLength(4)]
    public string ATMCardPIN { get; set; } = string.Empty;

    public DateTime ATMCardIssueDate { get; set; }

    public DateTime ATMCardExpiryDate { get; set; }

    [Required]
    public Guid CustomerId { get; set; }

    public DateTime DateCreated { get; set; } = DateTime.Now;

    public DateTime DateUpdated { get; set; } = DateTime.Now;

    [Precision(18, 2)]
    public decimal Balance { get; set; } = 0;

    public Customer Customer { get; set; }

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}

public enum AccountType
{
    Savings,
    Current,
    Domiciliary
}

public enum CardType
{
    NA,
    MasterCard,
    Visa,
    Verve
}