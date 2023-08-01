using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using ZenithBankATM.API.Exceptions;
using ZenithBankATM.API.Models;

namespace ZenithBankATM.API.Infrastructures.Filters;

public class ApiExceptionFilter : IActionFilter, IOrderedFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public int Order => int.MaxValue - 10;

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        string errorMessage = string.Empty;
        if (context.Exception is ApiException httpResponseException)
        {
            errorMessage = httpResponseException.Message;
            context.Result = new ObjectResult(
                new ApiResponse<ApiException>(true, errorMessage))
            {
                StatusCode = httpResponseException.StatusCode
            };

            context!.ExceptionHandled = true;

            _logger.LogError(context.Exception, errorMessage);
            return;
        }
        if (context.Exception is Exception exception)
        {
            errorMessage = exception?.Message ?? "Unknown error occurred";
            context.Result = new ObjectResult(
                new ApiResponse<object>(true, errorMessage))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context!.ExceptionHandled = true;

            _logger.LogError(context.Exception, errorMessage);
            return;
        }
    }
}
