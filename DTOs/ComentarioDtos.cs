
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class ComentarioDto
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Texto { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
}

public class CreateComentarioDto
{
    [Required]
    public string Texto { get; set; } = string.Empty;
}
