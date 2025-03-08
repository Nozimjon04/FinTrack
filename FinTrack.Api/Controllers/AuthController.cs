using FinTrack.Api.Helpers;
using FinTrack.Api.Service.DTOs.Users;
using FinTrack.Api.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

public class AuthController(IUserService userService) : BaseController
{
    [HttpPost("login")]
    public async Task<IActionResult> LoginAsync(string email, string password, CancellationToken cancellationToken)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.LoginAsync(email, password, cancellationToken)
        });

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserForCreationDto dto, CancellationToken cancellationToken)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RegisterAsync(dto, cancellationToken)
        });
}
