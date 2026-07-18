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
    GetVehicleByIdHandler getVehicleByIdHandler) : ApiController
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
}
