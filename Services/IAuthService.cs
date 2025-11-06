
using construtivaBack.DTOs;
using Microsoft.AspNetCore.Identity;

namespace construtivaBack.Services;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(RegisterDto model);
    Task<UserTokenDto?> LoginUserAsync(LoginDto model);
}
