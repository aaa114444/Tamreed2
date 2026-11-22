using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Tamreed2.Application.CallbackRequests.Commands.CreateCallbackRequest;
using Tamreed2.Application.CallbackRequests.Commands.UpdateCallbackRequestStatus;
using Tamreed2.Application.CallbackRequests.Queries.GetAllCallbackRequests;
using Tamreed2.Contracts.CallbackRequests;
using Tamreed2.Domain.CallbackRequests;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class CallbackRequestsController : ControllerBase
{
    private readonly ISender _sender;

    public CallbackRequestsController(ISender sender)
    {
        _sender = sender;
    }

    // POST: api/callbackrequests
    // Public endpoint. Submitting a callback request does not create a booking.
    [AllowAnonymous]
    [HttpPost]
    [EnableRateLimiting("CallbackRequests")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    public async Task<IActionResult> CreateCallbackRequest(
        [FromBody] CreateCallbackRequestBody request,
        CancellationToken cancellationToken)
    {
        // Honeypot: bots that fill this hidden field receive a fake success response.
        if (!string.IsNullOrWhiteSpace(request.Website))
        {
            return Accepted(new
            {
                Message = "Your callback request has been received."
            });
        }

        var requestId = await _sender.Send(
            new CreateCallbackRequestCommand(
                request.Name,
                request.MobileNumber,
                request.ServicesNeeded,
                request.Message),
            cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new
        {
            Id = requestId,
            Message = "Your callback request has been received. Our team will contact you."
        });
    }

    // GET: api/callbackrequests
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<CallbackRequestResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllCallbackRequests(
        CancellationToken cancellationToken)
    {
        var requests = await _sender.Send(
            new GetAllCallbackRequestsQuery(),
            cancellationToken);

        return Ok(requests);
    }

    // PUT: api/callbackrequests/{id}/status
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(
        Guid id,
        [FromBody] UpdateCallbackRequestStatusBody request,
        CancellationToken cancellationToken)
    {
        if (!Enum.TryParse<CallbackRequestStatus>(
                request.Status,
                ignoreCase: true,
                out var status)
            || !Enum.IsDefined(status))
        {
            return BadRequest(new
            {
                Message = "Invalid status. Allowed values: New, Contacted, Completed."
            });
        }

        await _sender.Send(
            new UpdateCallbackRequestStatusCommand(id, status),
            cancellationToken);

        return NoContent();
    }
}

public sealed record CreateCallbackRequestBody(
    string Name,
    string MobileNumber,
    IReadOnlyList<string> ServicesNeeded,
    string? Message,
    string? Website = null);

public sealed record UpdateCallbackRequestStatusBody(string Status);