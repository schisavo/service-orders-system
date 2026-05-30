using System.ComponentModel.DataAnnotations;

namespace ServiceOrdersSystem.Application.DTOs;

public class ClientDto
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    public string DocumentNumber { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    [Phone]
    public string Phone { get; set; } = string.Empty;
}
