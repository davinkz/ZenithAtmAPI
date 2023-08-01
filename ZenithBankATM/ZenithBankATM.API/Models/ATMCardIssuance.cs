using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Utils;

namespace ZenithBankATM.API.Models;

public class ATMCardIssuance
{
    public string Number { get; set; }
    public string CVV { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string PIN { get; set; }

    private ATMCardIssuance()
    {
        Number = string.Empty;
        CVV = string.Empty;
        IssueDate = default;
        ExpiryDate = default;
        PIN = string.Empty;
    }

    private ATMCardIssuance(string number,
        string cvv,
        DateTime issueDate,
        DateTime expiryDate,
        string pin)
    {
        Number = number;
        CVV = cvv;
        IssueDate = issueDate;
        ExpiryDate = expiryDate;
        PIN = pin;
    }

    public static ATMCardIssuance New(CardType cardType)
    {
        if (cardType == CardType.NA)
            return new ATMCardIssuance(
                string.Empty,
                string.Empty,
                default,
                default,
                string.Empty);

        string carNumber = cardType switch
        {
            CardType.MasterCard => $"50{StringUtils.GenerateRandomDigits(14)}",
            CardType.Visa => $"55{StringUtils.GenerateRandomDigits(14)}",
            CardType.Verve => $"4{StringUtils.GenerateRandomDigits(19)}",
            _ => throw new NotImplementedException()
        };
        string cvv = StringUtils.GenerateRandomDigits(3);
        string pin = StringUtils.GenerateRandomDigits(4);
        DateTime issueDate = DateTime.Now;
        DateTime expiryDate = cardType switch
        {
            CardType.MasterCard => issueDate.AddYears(5),
            _ => issueDate.AddYears(4)
        };

        return new ATMCardIssuance(carNumber, cvv, issueDate, expiryDate, pin);
    }
}
