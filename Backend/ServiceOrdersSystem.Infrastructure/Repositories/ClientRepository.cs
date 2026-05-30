using System.Data;
using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Application.Interfaces;

namespace ServiceOrdersSystem.Infrastructure.Repositories;

public class ClientRepository : IClientRepository
{
    private readonly IDbConnection _db;

    public ClientRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Client>> GetAllAsync()
    {
        var sql = @"SELECT * FROM Clients ORDER BY Id";
        return await _db.QueryAsync<Client>(sql);
    }

    public async Task<Client?> GetByIdAsync(int id)
    {
        var sql = @"SELECT * FROM Clients WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Client>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(Client client)
    {
        var sql = @"
            INSERT INTO Clients
            (FullName, DocumentNumber, Address, Phone)
            VALUES (@FullName, @DocumentNumber, @Address, @Phone)
            RETURNING Id";

        return await _db.ExecuteScalarAsync<int>(sql, client);
    }

    public async Task<bool> UpdateAsync(Client client)
    {
        var sql = @"
            UPDATE Clients
            SET FullName = @FullName,
                DocumentNumber = @DocumentNumber,
                Address = @Address,
                Phone = @Phone
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, client);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = @"DELETE FROM Clients WHERE Id = @Id";
        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
