using Microsoft.EntityFrameworkCore;

namespace ZenithBankATM.API.Services.Onboarding;

internal sealed partial class OnboardingService
{
    private async Task<bool> EmailExists(string email) =>
        await _storageDataContext
        .Customers
        .AnyAsync(x => x.Email == email);
}
