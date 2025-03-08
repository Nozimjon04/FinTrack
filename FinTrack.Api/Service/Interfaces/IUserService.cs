using FinTrack.Api.Service.DTOs.Users;

namespace FinTrack.Api.Service.Interfaces;

public interface IUserService
{
    public Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default);
    public Task<string> RegisterAsync(UserForCreationDto dto, CancellationToken cancellationToken = default);
    public Task<UserForResultDto> RetrieveByIdAsync(CancellationToken cancellationToken = default);
    public Task<bool> UpdateAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default);

}
