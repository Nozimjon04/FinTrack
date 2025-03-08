using AutoMapper;
using FinTrack.Api.Data.IRepositories;
using FinTrack.Api.Domain.Models;
using FinTrack.Api.Helpers;
using FinTrack.Api.Service.DTOs.Users;
using FinTrack.Api.Service.Exceptions;
using FinTrack.Api.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FinTrack.Api.Service.Services;

public class UserService : IUserService
{
    private readonly IMapper mapper;
    private readonly IAuthService authService;
    private readonly IRepository<User> userRepository;
    public UserService(IMapper mapper, IRepository<User> userRepository, IAuthService authService)
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.authService = authService;
    }

    public async Task<string> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var user = await this.userRepository.SelectAll()
            .Where(u => u.Email == email)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new CustomException(404, "Invalid email or password");

        return authService.GenerateToken(user);
    }

    public async Task<string> RegisterAsync(UserForCreationDto dto, CancellationToken cancellationToken = default)
    {
        var user = await this.userRepository.SelectAll()
            .Where(u => u.Email == dto.Email)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (user is not null)
            throw new CustomException(409, $"{dto.Email} is already exists");

        var mappedEntity = this.mapper.Map<User>(dto);
        mappedEntity.CreatedAt = DateTime.UtcNow;
        mappedEntity.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);
        var insertedUser = await this.userRepository.InsertAsync(mappedEntity, cancellationToken);
        await this.userRepository.SaveChangeAsync(cancellationToken);

        return authService.GenerateToken(insertedUser);
    }

    public async Task<UserForResultDto> RetrieveByIdAsync(CancellationToken cancellationToken = default)
    {
        long id = HttpContextHelper.UserId ?? 0;
        var user = await this.userRepository.SelectAll()
            .Where(u => u.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            throw new CustomException(404, $"User with {id} not found");

        return this.mapper.Map<UserForResultDto>(user);
    }

    public async Task<bool> UpdateAsync(long id, UserForUpdateDto dto, CancellationToken cancellationToken = default)
    {
        var user = await this.userRepository.SelectAll()
            .Where(u => u.Id == id)
            .FirstOrDefaultAsync(cancellationToken);
        if (user is null)
            throw new CustomException(404, $"User with {id} not found");

        var mappedEntity = this.mapper.Map(dto, user);

        return await this.userRepository.SaveChangeAsync(cancellationToken);
    }
}
