using FinTrack.Api.Domain.Models;
using FinTrack.Api.Service.DTOs.Users;

namespace FinTrack.Api.Service.Interfaces;

public interface IAuthService
{
    public string GenerateToken(User user);
}
