using System.Data;
using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Application.Interfaces;

namespace ServiceOrdersSystem.Infrastructure.Repositories;

public class TechnicianRepository : ITechnicianRepository
{
    private readonly IDbConnection _db;

    public TechnicianRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Technician>> GetAllAsync()
    {
        var sql = @"
            SELECT *
            FROM Technicians
            ORDER BY Id";

        return await _db.QueryAsync<Technician>(sql);
    }

    public async Task<Technician?> GetByIdAsync(int id)
    {
        var sql = @"
            SELECT *
            FROM Technicians
            WHERE Id = @Id";

        return await _db.QueryFirstOrDefaultAsync<Technician>(
            sql,
            new { Id = id }
        );
    }

    public async Task<int> CreateAsync(Technician technician)
    {
        var sql = @"
            INSERT INTO Technicians
            (
                FullName,
                Phone,
                Specialty
            )
            VALUES
            (
                @FullName,
                @Phone,
                @Specialty
            )
            RETURNING Id";

        return await _db.ExecuteScalarAsync<int>(
            sql,
            technician
        );
    }

    public async Task<bool> UpdateAsync(Technician technician)
    {
        var sql = @"
            UPDATE Technicians
            SET
                FullName = @FullName,
                Phone = @Phone,
                Specialty = @Specialty
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, technician);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var sql = @"
            DELETE FROM Technicians
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, new { Id = id });
        return rows > 0;
    }
}
