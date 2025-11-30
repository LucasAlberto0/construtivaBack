using construtivaBack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace construtivaBack.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsersController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("me/profile-picture")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile profilePicture)
    {
        // 1. Get User
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null) return Unauthorized();
        
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound("User not found.");

        // 2. Validate File
        if (profilePicture == null || profilePicture.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        if (profilePicture.Length > 5 * 1024 * 1024) // 5 MB
        {
            return StatusCode(413, "File size exceeds the 5MB limit.");
        }

        var allowedMimeTypes = new[] { "image/jpeg", "image/png", "image/webp" };
        if (!allowedMimeTypes.Contains(profilePicture.ContentType.ToLower()))
        {
            return BadRequest("Invalid file type. Only JPG, PNG, and WEBP are allowed.");
        }

        // 3. Convert file to byte array and update user
        using (var memoryStream = new MemoryStream())
        {
            await profilePicture.CopyToAsync(memoryStream);
            user.ProfilePictureData = memoryStream.ToArray();
            user.ProfilePictureMimeType = profilePicture.ContentType;
        }

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Ok(new { message = "Profile picture updated successfully." });
    }

    [HttpGet("{userId}/profile-picture")]
    [AllowAnonymous] // Allow anonymous to make it easier for img tags to fetch the picture
    public async Task<IActionResult> GetUserProfilePicture(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.ProfilePictureData == null || user.ProfilePictureMimeType == null)
        {
            return NotFound();
        }

        return File(user.ProfilePictureData, user.ProfilePictureMimeType);
    }
}
