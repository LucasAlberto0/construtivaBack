using construtivaBack.DTOs;
using construtivaBack.Models;
using construtivaBack.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace construtivaBack.Controllers;

[Authorize(Roles = "Administrador")]
[Route("api/[controller]")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly IAdminService _adminService;

    public AdminController(IAdminService adminService)
    {
        _adminService = adminService;
    }

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserRolesDto>>> GetUsersWithRoles()
    {
        var userRolesDto = await _adminService.GetUsersWithRolesAsync();
        return Ok(userRolesDto);
    }

    [HttpPost("assign-role")]
    public async Task<IActionResult> AssignRole([FromBody] UpdateUserRoleDto model)
    {
        var result = await _adminService.AssignRoleAsync(model);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Description == "User not found." || e.Description == "Role does not exist."))
            {
                return NotFound(result.Errors.First().Description);
            }
            return BadRequest(result.Errors);
        }
        return Ok(new { message = $"Role '{model.RoleName}' assigned to user '{model.Email}' successfully." });
    }

    [HttpPost("remove-role")]
    public async Task<IActionResult> RemoveRole([FromBody] UpdateUserRoleDto model)
    {
        var result = await _adminService.RemoveRoleAsync(model);
        if (!result.Succeeded)
        {
            if (result.Errors.Any(e => e.Description == "User not found." || e.Description == "Role does not exist."))
            {
                return NotFound(result.Errors.First().Description);
            }
            return BadRequest(result.Errors);
        }
        return Ok(new { message = $"Role '{model.RoleName}' removed from user '{model.Email}' successfully." });
    }
}