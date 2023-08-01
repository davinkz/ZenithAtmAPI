using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Models;

namespace ZenithBankATM.API.Services.Onboarding;

public interface IOnboardingService
{
    Task<Customer> CreateCustomer(
        CustomerCreationRequest request,
        CancellationToken cancellationToken = default);

    Task<CustomerData?> ConfirmCustomerAndCreateAccount(
        AccountConfirmationRequest request,
        CancellationToken cancellationToken = default);
}
