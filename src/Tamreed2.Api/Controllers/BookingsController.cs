using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tamreed2.Application.Bookings.Commands.CancelBooking;
using Tamreed2.Application.Bookings.Commands.CompleteBooking;
using Tamreed2.Application.Bookings.Commands.ConfirmBooking;
using Tamreed2.Application.Bookings.Commands.CreateBooking;
using Tamreed2.Application.Bookings.Commands.StartBooking;
using Tamreed2.Application.Bookings.Queries.GetAllBookings;
using Tamreed2.Application.Bookings.Queries.GetMyBookings;
using Tamreed2.Contracts.Bookings;

namespace Tamreed2.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class BookingsController : ControllerBase
{
    private readonly ISender _sender;

    public BookingsController(ISender sender)
    {
        _sender = sender;
    }

    // POST: api/bookings
    [Authorize(Roles = "User")]
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CreateBooking(
        [FromBody] CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateBookingCommand(
            request.ServiceId,
            request.ScheduledAtUtc,
            request.City,
            request.District,
            request.Street,
            request.BuildingNumber,
            request.ApartmentNumber,
            request.Notes);

        var bookingId = await _sender.Send(command, cancellationToken);

        return StatusCode(StatusCodes.Status201Created, new
        {
            Id = bookingId,
            Message = "Booking created successfully."
        });
    }

    // GET: api/bookings/my
    [Authorize(Roles = "User")]
    [HttpGet("my")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetMyBookings(
        CancellationToken cancellationToken)
    {
        var bookings = await _sender.Send(
            new GetMyBookingsQuery(),
            cancellationToken);

        return Ok(bookings);
    }

    // GET: api/bookings
    [Authorize(Roles = "Admin")]
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllBookings(
        CancellationToken cancellationToken)
    {
        var bookings = await _sender.Send(
            new GetAllBookingsQuery(),
            cancellationToken);

        return Ok(bookings);
    }

    // PUT: api/bookings/{id}/confirm
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/confirm")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> ConfirmBooking(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new ConfirmBookingCommand(id),
            cancellationToken);

        return NoContent();
    }

    // PUT: api/bookings/{id}/start
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/start")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> StartBooking(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new StartBookingCommand(id),
            cancellationToken);

        return NoContent();
    }

    // PUT: api/bookings/{id}/complete
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CompleteBooking(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new CompleteBookingCommand(id),
            cancellationToken);

        return NoContent();
    }

    // PUT: api/bookings/{id}/cancel
    [Authorize(Roles = "Admin")]
    [HttpPut("{id:guid}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> CancelBooking(
        Guid id,
        CancellationToken cancellationToken)
    {
        await _sender.Send(
            new CancelBookingCommand(id),
            cancellationToken);

        return NoContent();
    }
}
