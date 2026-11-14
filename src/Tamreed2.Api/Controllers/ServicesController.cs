using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Services.Commands.CreateService;
using Tamreed2.Application.Services.Commands.UpdateService;
using Tamreed2.Application.Services.Queries.GetAllServices;
using Tamreed2.Application.Services.Queries.GetServiceById;
using Tamreed2.Contracts.Services;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ServicesController : ControllerBase
{
    private readonly ISender _sender;

    public ServicesController(ISender sender)
    {
        _sender = sender;
    }

    // POST: api/services (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateService(
        [FromBody] CreateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateServiceCommand(
            request.Name,
            request.Description,
            request.Price);

        var serviceId = await _sender.Send(command, cancellationToken);

        return Created(
            $"/api/services/{serviceId}",
            new { Id = serviceId });
    }

    // GET: api/services/{id} (Public)
    [AllowAnonymous]
    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetServiceById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var service = await _sender.Send(
            new GetServiceByIdQuery(id),
            cancellationToken);

        if (service is null)
        {
            return NotFound(new
            {
                Message = $"Service with ID '{id}' was not found."
            });
        }

        return Ok(service);
    }

    // GET: api/services (Public)
    [AllowAnonymous]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ServiceResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllServices(
        CancellationToken cancellationToken)
    {
        var services = await _sender.Send(
            new GetAllServicesQuery(),
            cancellationToken);

        return Ok(services);
    }

    // PUT: api/services/{id} (Admin only)
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> UpdateService(
        Guid id,
        [FromBody] UpdateServiceRequest request,
        CancellationToken cancellationToken)
    {
        var command = new UpdateServiceCommand(
            id,
            request.Name,
            request.Description,
            request.Price);

        var serviceId = await _sender.Send(command, cancellationToken);

        return Ok(new
        {
            Id = serviceId,
            Message = "Service updated successfully."
        });
    }
}
