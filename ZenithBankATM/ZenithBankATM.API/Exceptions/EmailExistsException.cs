namespace ZenithBankATM.API.Exceptions;

public class EmailExistsException : ApiException
{
    public EmailExistsException(string source, string email)
        : base(
            StatusCodes.Status400BadRequest,
            $"There is already a {source} with this email address: {email}")
    { }
}
