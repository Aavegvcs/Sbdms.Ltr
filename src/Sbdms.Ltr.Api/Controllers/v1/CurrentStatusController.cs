using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.CurrentStatus;
using Sbdms.Ltr.Core.Feature.CurrentStatuses;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class CurrentStatusController(
    AddCurrentStatusHandler addCurrentStatusHandler,
    UpdateCurrentStatusHandler updateCurrentStatusHandler,
    GetAllCurrentStatusesHandler getAllCurrentStatusesHandler,
    GetCurrentStatusByIdHandler getCurrentStatusByIdHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddCurrentStatus([FromBody] AddCurrentStatusRequest request)
    {
        var response = await addCurrentStatusHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCurrentStatus(Guid id, [FromBody] UpdateCurrentStatusRequest request)
    {
        if (id != request.CurrentStatusId)
            return BadRequest("Route id and request id must match.");

        var response = await updateCurrentStatusHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllCurrentStatuses()
    {
        var response = await getAllCurrentStatusesHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCurrentStatusById(Guid id)
    {
        var response = await getCurrentStatusByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
