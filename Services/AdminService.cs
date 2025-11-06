
using construtivaBack.DTOs;
using construtivaBack.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Services;

public class AdminService : IAdminService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public AdminService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<IEnumerable<UserRolesDto>> GetUsersWithRolesAsync()
    {
        var users = await _userManager.Users.ToListAsync();
        var userRolesDto = new List<UserRolesDto>();

        foreach (var user in users)
        {
            userRolesDto.Add(new UserRolesDto
            {
                UserId = user.Id,
                Email = user.Email,
                Roles = await _userManager.GetRolesAsync(user)
            });
        }
        return userRolesDto;
    }

    public async Task<IdentityResult> AssignRoleAsync(UpdateUserRoleDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        if (!await _roleManager.RoleExistsAsync(model.RoleName))
        {
            return IdentityResult.Failed(new IdentityError { Description = "Role does not exist." });
        }

        var result = await _userManager.AddToRoleAsync(user, model.RoleName);
        return result;
    }

    public async Task<IdentityResult> RemoveRoleAsync(UpdateUserRoleDto model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found." });
        }

        var result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
        return result;
    }
}
