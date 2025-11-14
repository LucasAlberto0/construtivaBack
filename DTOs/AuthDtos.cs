
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "O campo Email é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O campo Senha é obrigatório.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
    
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "As senhas não conferem.")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string? NomeCompleto { get; set; }
}

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class UserTokenDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime Expiration { get; set; }
}
