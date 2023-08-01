namespace ZenithBankATM.API.Models;

public record ApiResponse<T>(
    bool HasError,
    string Message,
    T? Data = default);
