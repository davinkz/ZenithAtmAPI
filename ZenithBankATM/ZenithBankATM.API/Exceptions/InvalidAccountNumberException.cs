namespace ZenithBankATM.API.Exceptions;

public class InvalidAccountNumberException : ApiException
{
    public InvalidAccountNumberException(string accountNumber) :
        base(
            StatusCodes.Status400BadRequest,
            $"Account number {accountNumber} is invalid.")
    { }
}
