using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sbdms.Ltr.Contracts.User;
using Sbdms.Ltr.Core.Feature.Users;

namespace Sbdms.Ltr.Api.Controllers.v1;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1.0")]
public class UserController(
    AddUserHandler addUserHandler,
    GetAllUsersHandler getAllUsersHandler,
    GetUserByIdHandler getUserByIdHandler,
    GetUserByMobileNumberHandler getUserByMobileNumberHandler) : ApiController
{
    [HttpPost]
    public async Task<IActionResult> AddUser([FromBody] AddUserRequest request)
    {
        var response = await addUserHandler.HandleAsync(request);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

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

    // The caller's own profile, resolved from their access token — no id in the route.
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;
        if (!int.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var response = await getUserByIdHandler.HandleAsync(userId);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }

    // Looks up a user's employeeCode/name by mobile number — e.g. to prefill a login form
    // before the user confirms and continues.
    [HttpGet("lookup/{mobileNumber}")]
    public async Task<IActionResult> GetUserByMobileNumber(string mobileNumber)
    {
        var response = await getUserByMobileNumberHandler.HandleAsync(mobileNumber);
        return response.Match(result => Ok(response.Value), errors => Problem(errors));
    }
}
