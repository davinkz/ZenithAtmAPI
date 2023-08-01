namespace ZenithBankATM.API.Exceptions;

public class CustomerNotFoundException : ApiException
{
    public CustomerNotFoundException(string? reason = null)
        : base(
            StatusCodes.Status404NotFound,
            string.IsNullOrWhiteSpace(reason) ? string.Empty : $" reason: {reason}")
    { }
}
