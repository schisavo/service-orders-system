using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Application.Interfaces;

public interface ITechnicianRepository
{
    Task<IEnumerable<Technician>> GetAllAsync();
    Task<Technician?> GetByIdAsync(int id);
    Task<int> CreateAsync(Technician technician);
    Task<bool> UpdateAsync(Technician technician);
    Task<bool> DeleteAsync(int id);
}
