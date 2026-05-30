namespace ServiceOrdersSystem.Domain.Entities;

public class ServiceOrder
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Status { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int TechnicianId { get; set; }

    public int ClientId { get; set; }
}