using System.ComponentModel.DataAnnotations;

namespace ServiceOrdersSystem.Application.DTOs;

public class ServiceOrderDto
{
    public int Id { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public int TechnicianId { get; set; }

    [Required]
    public int ClientId { get; set; }
}
