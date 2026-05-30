using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Application.Interfaces;

public interface IServiceOrderRepository
{
    Task<IEnumerable<ServiceOrder>> GetAllAsync();
    Task<ServiceOrder?> GetByIdAsync(int id);
    Task<int> CreateAsync(ServiceOrder order);
    Task<bool> UpdateAsync(ServiceOrder order);
    Task<bool> DeleteAsync(int id);
    Task<IEnumerable<ServiceOrder>> FilterOrders(
        string? estado,
        string? tecnico,
        string? especialidad,
        string? cliente,
        string? documento,
        DateTime? fechaInicio,
        DateTime? fechaFin
    );
}
