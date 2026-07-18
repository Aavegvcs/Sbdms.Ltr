using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.Driver;
using Sbdms.Ltr.Core.Feature.Drivers;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class DriverController(
    AddDriverHandler addDriverHandler,
    UpdateDriverHandler updateDriverHandler,
    GetAllDriversHandler getAllDriversHandler,
    GetDriverByIdHandler getDriverByIdHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddDriver([FromBody] AddDriverRequest request)
    {
        var response = await addDriverHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateDriver(int id, [FromBody] UpdateDriverRequest request)
    {
        if (id != request.Id)
            return BadRequest("Route id and request id must match.");

        var response = await updateDriverHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllDrivers()
    {
        var response = await getAllDriversHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDriverById(int id)
    {
        var response = await getDriverByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
