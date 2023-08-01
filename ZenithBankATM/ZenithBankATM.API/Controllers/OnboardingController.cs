using Microsoft.AspNetCore.Mvc;

using ZenithBankATM.API.Database.Entities;
using ZenithBankATM.API.Models;
using ZenithBankATM.API.Services.Onboarding;

namespace ZenithBankATM.API.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class OnboardingController : ControllerBase
{
    private readonly ILogger<OnboardingController> _logger;
    private readonly IOnboardingService _customerService;

    public OnboardingController(
        ILogger<OnboardingController> logger,
        IOnboardingService customerService)
    {
        _logger = logger;
        _customerService = customerService;
    }

    /// <summary>
    /// Creates a new account
    /// </summary>
    /// <returns></returns>
    [HttpPost, Route("Register")]
    [ProducesResponseType(typeof(ApiResponse<Customer>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] CustomerCreationRequest request)
    {
        _logger.LogInformation("Receieved customer creation request {request}", request);

        Customer customer = await _customerService.CreateCustomer(request);

        _logger.LogInformation("New customer created {customer}", customer);

        var respons = new ApiResponse<Customer>(false, "Customer creation was successful.", customer);
        return Ok(respons);
    }

    [HttpPut, Route("Confirm-Customer-And-Generate-Account-Number")]
    [ProducesResponseType(typeof(ApiResponse<CustomerData>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ConfirmCustomerAndGenerateAccountNumber(
        [FromBody] AccountConfirmationRequest request)
    {
        _logger.LogInformation("Receieved customer account confirmation request {request}", request);

        CustomerData? customer = await _customerService.ConfirmCustomerAndCreateAccount(request);

        _logger.LogInformation("Customer confirmed {customer}", customer);

        var response = new ApiResponse<CustomerData>(false, "Customer confirmed successfully.", customer);
        return Ok(response);
    }
}
