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

    [Required(ErrorMessage = "O campo Perfil é obrigatório.")]
    [RegularExpression("^(Admin|Coordenador|Fiscal)$", ErrorMessage = "O perfil deve ser 'Admin', 'Coordenador' ou 'Fiscal'.")]
    public string Role { get; set; } = string.Empty;
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

public class UserInfoDto
{
    public string Id { get; set; }
    public string Email { get; set; }
    public string? NomeCompleto { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public IList<string> Roles { get; set; }
}

public class UpdateUserDto
{
    [EmailAddress(ErrorMessage = "O formato do email é inválido.")]
    public string? Email { get; set; }

    public string? NomeCompleto { get; set; }

    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "As senhas não conferem.")]
    public string? ConfirmNewPassword { get; set; }
}