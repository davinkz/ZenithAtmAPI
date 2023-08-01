namespace ZenithBankATM.API.Exceptions;

public class AccountNumberNotFoundException : ApiException
{
    public AccountNumberNotFoundException(string accountNumber) :
        base(
            StatusCodes.Status404NotFound,
            $"Account number {accountNumber} does not exists.")
    { }
}
