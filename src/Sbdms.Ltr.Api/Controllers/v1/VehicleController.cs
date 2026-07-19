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
    GetVehicleQrImageHandler getVehicleQrImageHandler) : ApiController
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
}
