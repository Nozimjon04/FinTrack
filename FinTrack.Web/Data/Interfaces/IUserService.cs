using FinTrack.Web.Helpers;
using FinTrack.Web.Models;

namespace FinTrack.Web.Data.Interfaces;

public interface IUserService
{
    public Task<UserForResultDto> RetrieveProfileAsync(CancellationToken cancellationToken = default);
    public Task<Response> UpdateProfileAsync(long id,UserForUpdateDto dto, CancellationToken cancellationToken = default);
}
