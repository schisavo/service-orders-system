using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByUsername(string username);
}
