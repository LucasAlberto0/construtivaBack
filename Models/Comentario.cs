
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.Models;

public class Comentario
{
    public int Id { get; set; }
    public int ObraId { get; set; }
    public string UserId { get; set; } = string.Empty;

    [Required]
    public string Texto { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Navigation Properties
    public virtual Obra? Obra { get; set; }
    public virtual ApplicationUser? User { get; set; }
}
