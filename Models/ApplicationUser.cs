
using Microsoft.AspNetCore.Identity;

namespace construtivaBack.Models;

public class ApplicationUser : IdentityUser
{
    public string? NomeCompleto { get; set; }
    public byte[]? ProfilePictureData { get; set; }
    public string? ProfilePictureMimeType { get; set; }
}
