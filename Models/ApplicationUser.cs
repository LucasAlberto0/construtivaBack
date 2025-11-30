
using Microsoft.AspNetCore.Identity;

namespace construtivaBack.Models;

public class ApplicationUser : IdentityUser
{
    // Aqui você pode adicionar propriedades personalizadas para o usuário,
    // como Nome Completo, Cargo, etc.
    public string? NomeCompleto { get; set; }
    public byte[]? ProfilePictureData { get; set; }
    public string? ProfilePictureMimeType { get; set; }
}
