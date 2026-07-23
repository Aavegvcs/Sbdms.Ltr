using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.Booking;
using Sbdms.Ltr.Core.Feature.Bookings;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class BookingController(
    GuestStartBookingHandler guestStartBookingHandler,
    CreateBookingHandler createBookingHandler,
    GetAllBookingsHandler getAllBookingsHandler,
    GetBookingByIdHandler getBookingByIdHandler,
    GetLatestBookingByUserHandler getLatestBookingByUserHandler,
    GetBookingHistoryByUserHandler getBookingHistoryByUserHandler,
    CompleteBookingHandler completeBookingHandler) : ApiController
{
    // Path A — new/unrecognized user scans the QR: identify/register by mobile number, log them
    // in, and create the booking, all in one call. No OTP.
    [HttpPost("guest/start")]
    [AllowAnonymous]
    public async Task<IActionResult> GuestStart([FromBody] GuestStartBookingRequest request)
    {
        var response = await guestStartBookingHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Path B — already-logged-in user scans the QR and books directly using their access token.
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateBooking([FromBody] CreateBookingRequest request)
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await createBookingHandler.HandleAsync(userId, request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // The current user's most recent booking (whatever its status).
    [HttpGet("me/latest")]
    [Authorize]
    public async Task<IActionResult> GetMyLatestBooking()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await getLatestBookingByUserHandler.HandleAsync(userId);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // The current user's past (completed/cancelled) bookings, most recent first.
    [HttpGet("me/history")]
    [Authorize]
    public async Task<IActionResult> GetMyBookingHistory()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await getBookingHistoryByUserHandler.HandleAsync(userId);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllBookings()
    {
        var response = await getAllBookingsHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookingById(int id)
    {
        var response = await getBookingByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Rider explicitly ends their own ride — hit by the user app. Only completes the
    // caller's own booking; other riders pooled into the same trip are unaffected.
    [HttpPost("{id:int}/complete")]
    [Authorize]
    public async Task<IActionResult> CompleteBooking(int id)
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await completeBookingHandler.HandleAsync(userId, id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
