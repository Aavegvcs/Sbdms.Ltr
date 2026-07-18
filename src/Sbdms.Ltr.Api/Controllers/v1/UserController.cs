using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Core.Feature.Users;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController(
    GetAllUsersHandler getAllUsersHandler,
    GetUserByIdHandler getUserByIdHandler) : ApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var response = await getAllUsersHandler.HandleAsync();
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var response = await getUserByIdHandler.HandleAsync(id);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
