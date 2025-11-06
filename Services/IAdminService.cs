
using construtivaBack.DTOs;
using Microsoft.AspNetCore.Identity;

namespace construtivaBack.Services;

public interface IAdminService
{
    Task<IEnumerable<UserRolesDto>> GetUsersWithRolesAsync();
    Task<IdentityResult> AssignRoleAsync(UpdateUserRoleDto model);
    Task<IdentityResult> RemoveRoleAsync(UpdateUserRoleDto model);
}
