using System.Text.Json;

using ZenithBankATM.API.Database.Entities;

namespace ZenithBankATM.API.Services.Onboarding;

public sealed record AccountConfirmationRequest(
    Guid CustomerId,
    string AccountName,
    AccountType AccountType,
    bool RequiresATMCard,
    CardType CardType = CardType.NA)
{
    public override string ToString() =>
        JsonSerializer.Serialize(this);
}