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
    GuestConfirmBookingHandler guestConfirmBookingHandler,
    CreateBookingHandler createBookingHandler,
    GetAllBookingsHandler getAllBookingsHandler,
    GetBookingByIdHandler getBookingByIdHandler) : ApiController
{
    // Path A, step 1 — new/unrecognized user scans the QR: identify/register by mobile number, send OTP.
    [HttpPost("guest/start")]
    [AllowAnonymous]
    public async Task<IActionResult> GuestStart([FromBody] GuestStartBookingRequest request)
    {
        var response = await guestStartBookingHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Path A, step 2 — verify OTP, log the user in, and create the booking in one step.
    [HttpPost("guest/confirm")]
    [AllowAnonymous]
    public async Task<IActionResult> GuestConfirm([FromBody] GuestConfirmBookingRequest request)
    {
        var response = await guestConfirmBookingHandler.HandleAsync(request);
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
}
