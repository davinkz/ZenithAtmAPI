using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZenithBankATM.API.Database.Entities;

public class Customer
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [Required]
    [StringLength(30)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string LastName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [StringLength(12)]
    public string BVN { get; set; } = string.Empty;

    [Required]
    public bool IsConfirmed { get; set; } = false;

    public DateTime DateCreated { get; set; } = DateTime.Now;
    public DateTime DateUpdated { get; set; } = DateTime.Now;

    //public ICollection<Account> Accounts { get; set; }
}
