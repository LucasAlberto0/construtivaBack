
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace construtivaBack.DTOs;

public class UploadFileDto
{
    [Required]
    public IFormFile File { get; set; } = default!;
}
