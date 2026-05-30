using System.ComponentModel.DataAnnotations;

namespace ServiceOrdersSystem.Application.DTOs;

public class TechnicianDto
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;

    [Required]
    public string Specialty { get; set; } = string.Empty;
}
