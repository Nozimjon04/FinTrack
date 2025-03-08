using FinTrack.Api.Helpers;
using FinTrack.Api.Service.DTOs.Users;
using FinTrack.Api.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FinTrack.Api.Controllers;

[Authorize]
public class UsersController(IUserService userService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> RetrieveByIdAsync(CancellationToken cancellationToken)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.RetrieveByIdAsync(cancellationToken)
        });
    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdateAsync(long id,UserForUpdateDto dto, CancellationToken cancellationToken)
        => Ok(new Response
        {
            Code = 200,
            Message = "Success",
            Data = await userService.UpdateAsync(id, dto, cancellationToken)
        });



}
