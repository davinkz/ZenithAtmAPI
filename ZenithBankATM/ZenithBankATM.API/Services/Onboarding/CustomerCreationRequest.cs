using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace ZenithBankATM.API.Services.Onboarding;

public sealed record CustomerCreationRequest(
    [Required, MinLength(3), MaxLength(30)]
    string FirstName,

    [Required, MinLength(3), MaxLength(30)]
    string LastName,

    [Required, EmailAddress]
    string Email)
{

    public override string ToString() =>
        JsonSerializer.Serialize(this);

}