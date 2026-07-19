using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.VehicleType;
using Sbdms.Ltr.Core.Feature.VehicleTypes;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VehicleTypeController(
    AddVehicleTypeHandler addVehicleTypeHandler,
    UpdateVehicleTypeHandler updateVehicleTypeHandler,
    GetAllVehicleTypesHandler getAllVehicleTypesHandler,
    GetVehicleTypeByIdHandler getVehicleTypeByIdHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddVehicleType([FromBody] AddVehicleTypeRequest request)
    {
        var response = await addVehicleTypeHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateVehicleType(Guid id, [FromBody] UpdateVehicleTypeRequest request)
    {
        if (id != request.VehicleTypeId)
            return BadRequest("Route id and request id must match.");

        var response = await updateVehicleTypeHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVehicleTypes()
    {
        var response = await getAllVehicleTypesHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetVehicleTypeById(Guid id)
    {
        var response = await getVehicleTypeByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
