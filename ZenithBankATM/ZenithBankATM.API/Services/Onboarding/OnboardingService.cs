using Microsoft.EntityFrameworkCore;

using ZenithBankATM.API.Database.DataContext;
using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Exceptions;
using ZenithBankATM.API.Models;
using ZenithBankATM.API.Utils;

namespace ZenithBankATM.API.Services.Onboarding;

internal sealed partial class OnboardingService : IOnboardingService
{
    private readonly StorageDataContext _storageDataContext;

    public OnboardingService(StorageDataContext storageDataContext)
    {
        _storageDataContext = storageDataContext;
    }

    public async Task<Customer> CreateCustomer(
        CustomerCreationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        if (await EmailExists(request.Email))
        {
            throw new EmailExistsException("Customer", request.Email);
        }

        Customer customer = new()
        {
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            BVN = StringUtils.GenerateRandomDigits(11)
        };

        _storageDataContext.Customers.Add(customer);
        await _storageDataContext.SaveChangesAsync(cancellationToken);

        return customer;
    }

    public async Task<CustomerData?> ConfirmCustomerAndCreateAccount(
        AccountConfirmationRequest request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(nameof(request));

        (Guid customerId,
            string accountName,
            AccountType accountType,
            bool requiresATMCard,
            CardType cardType) = request;
        Customer customer = await _storageDataContext
            .Customers
            .FirstOrDefaultAsync(x => x.Id == customerId)
            ?? throw new CustomerNotFoundException();
        if (!customer.IsConfirmed)
        {
            ATMCardIssuance card = ATMCardIssuance.New(cardType);
            Database.Entities.Account account = new()
            {
                CustomerId = customerId,
                AccountNumber = StringUtils.GenerateRandomDigits(10),
                AccountType = accountType,
                ATMCardCCV = card.CVV,
                ATMCardExpiryDate = card.ExpiryDate,
                ATMCardIssueDate = card.IssueDate,
                ATMCardNumber = card.Number,
                ATMCardPIN = card.PIN,
                ATMCardType = cardType,
                HasATMCard = requiresATMCard,
                Name = accountName
            };

            _storageDataContext.Accounts.Add(account);
            await _storageDataContext.SaveChangesAsync(cancellationToken);
        }

        List<Database.Entities.Account> accounts = await _storageDataContext
                .Accounts
                .AsNoTracking()
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();

        return new CustomerData(
            customerId,
            customer.FirstName,
            customer.LastName,
            customer.Email,
            customer.BVN,
            accounts);
    }
}
