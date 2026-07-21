using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.Vendor;
using Sbdms.Ltr.Core.Feature.Vendors;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class VendorController(
    AddVendorHandler addVendorHandler,
    UpdateVendorHandler updateVendorHandler,
    GetAllVendorsHandler getAllVendorsHandler,
    GetVendorByIdHandler getVendorByIdHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddVendor([FromBody] AddVendorRequest request)
    {
        var response = await addVendorHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateVendor(int id, [FromBody] UpdateVendorRequest request)
    {
        if (id != request.Id)
            return BadRequest("Route id and request id must match.");

        var response = await updateVendorHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllVendors()
    {
        var response = await getAllVendorsHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVendorById(int id)
    {
        var response = await getVendorByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
