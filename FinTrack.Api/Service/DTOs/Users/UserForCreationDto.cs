namespace FinTrack.Api.Service.DTOs.Users;

public class UserForCreationDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}
