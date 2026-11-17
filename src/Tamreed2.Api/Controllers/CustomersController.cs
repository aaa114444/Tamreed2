using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Customers.Commands.SetCustomerActiveStatus;
using Tamreed2.Application.Customers.Queries.GetAllCustomers;
using Tamreed2.Contracts.Customers;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public sealed class CustomersController : ControllerBase
{
    private readonly ISender _sender;

    public CustomersController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CustomerResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllCustomers(
        [FromQuery] string? search,
        CancellationToken cancellationToken)
    {
        var customers = await _sender.Send(
            new GetAllCustomersQuery(search),
            cancellationToken);

        return Ok(customers);
    }

    [HttpPatch("{customerId:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> SetCustomerActiveStatus(
        Guid customerId,
        [FromQuery] bool isActive,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new SetCustomerActiveStatusCommand(customerId, isActive),
            cancellationToken);

        return NoContent();
    }
}
