using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.Vehicle;
using Sbdms.Ltr.Core.Feature.Vehicles;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VehicleController(
    AddVehicleHandler addVehicleHandler,
    UpdateVehicleHandler updateVehicleHandler,
    GetAllVehiclesHandler getAllVehiclesHandler,
    GetVehicleByIdHandler getVehicleByIdHandler,
    GetVehicleByQrCodeHandler getVehicleByQrCodeHandler,
    GetVehicleQrImageHandler getVehicleQrImageHandler,
    RegenerateVehicleQrCodeHandler regenerateVehicleQrCodeHandler,
    ChangeVehicleDriverHandler changeVehicleDriverHandler,
    GetVehicleDriverHistoryHandler getVehicleDriverHistoryHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddVehicle([FromBody] AddVehicleRequest request)
    {
        var response = await addVehicleHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateVehicle(int id, [FromBody] UpdateVehicleRequest request)
    {
        if (id != request.Id)
            return BadRequest("Route id and request id must match.");

        var response = await updateVehicleHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVehicles()
    {
        var response = await getAllVehiclesHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVehicleById(int id)
    {
        var response = await getVehicleByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Public: lets a scanning app show vehicle details right after a QR scan, before booking.
    [HttpGet("qr/{qrCode}")]
    public async Task<IActionResult> GetVehicleByQrCode(string qrCode)
    {
        var response = await getVehicleByQrCodeHandler.HandleAsync(qrCode);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Generates a printable QR code image encoding this vehicle's QrUniqueCode.
    [HttpGet("{id:int}/qr-image")]
    public async Task<IActionResult> GetVehicleQrImage(int id)
    {
        var result = await getVehicleQrImageHandler.HandleAsync(id);
        if (result.IsError)
            return Problem(result.Errors);

        return File(result.Value, "image/png");
    }

    // Rotates this vehicle's QrUniqueCode to a new value — the old QR sticker stops working.
    [HttpPost("{id:int}/qr/regenerate")]
    public async Task<IActionResult> RegenerateVehicleQrCode(int id)
    {
        var response = await regenerateVehicleQrCodeHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Re-pairs this vehicle with a (possibly different, possibly no) driver. The mapping being
    // replaced is snapshotted into VehicleDriverAssignmentLogs before it's overwritten.
    [HttpPut("{id:int}/driver")]
    public async Task<IActionResult> ChangeVehicleDriver(int id, [FromBody] ChangeVehicleDriverRequest request)
    {
        var response = await changeVehicleDriverHandler.HandleAsync(id, request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // History of this vehicle's driver re-assignments, most recent first.
    [HttpGet("{id:int}/driver-history")]
    public async Task<IActionResult> GetVehicleDriverHistory(int id)
    {
        var response = await getVehicleDriverHistoryHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
