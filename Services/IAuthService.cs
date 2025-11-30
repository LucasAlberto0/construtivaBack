using construtivaBack.DTOs;
using Microsoft.AspNetCore.Identity;

namespace construtivaBack.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto model);
    Task<UserTokenDto?> LoginUserAsync(LoginDto model);
    Task<UserInfoDto?> GetUserInfoAsync(string userId);
    Task<IdentityResult> UpdateUserAsync(string userId, UpdateUserDto model);
}