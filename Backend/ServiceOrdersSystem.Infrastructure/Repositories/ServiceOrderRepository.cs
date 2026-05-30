using System.Data;
using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Application.Interfaces;

namespace ServiceOrdersSystem.Infrastructure.Repositories;

public class ServiceOrderRepository : IServiceOrderRepository
{
    private readonly IDbConnection _db;

    public ServiceOrderRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<IEnumerable<ServiceOrder>> GetAllAsync()
    {
        var sql = @"SELECT * FROM ServiceOrders ORDER BY Id";
        return await _db.QueryAsync<ServiceOrder>(sql);
    }

    public async Task<ServiceOrder?> GetByIdAsync(int id)
    {
        var sql = @"SELECT * FROM ServiceOrders WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<ServiceOrder>(sql, new { Id = id });
    }

    public async Task<int> CreateAsync(ServiceOrder order)
    {
        var sql = @"
            INSERT INTO ServiceOrders
            (CreatedAt, Status, Description, TechnicianId, ClientId)
            VALUES (@CreatedAt, @Status, @Description, @TechnicianId, @ClientId)
            RETURNING Id";

        return await _db.ExecuteScalarAsync<int>(sql, order);
    }

    public async Task<bool> UpdateAsync(ServiceOrder order)
    {
        var sql = @"
            UPDATE ServiceOrders
            SET CreatedAt = @CreatedAt,
                Status = @Status,
                Description = @Description,
                TechnicianId = @TechnicianId,
                ClientId = @ClientId
            WHERE Id = @Id";

        var rows = await _db.ExecuteAsync(sql, order);
        return rows > 0;
    }

    public async Task<bool> DeleteAsync(int id)
        {
            var sql = @"DELETE FROM ServiceOrders WHERE Id = @Id";
            var rows = await _db.ExecuteAsync(sql, new { Id = id });
            return rows > 0;
        }

        public async Task<IEnumerable<ServiceOrder>> FilterOrders(
        string? estado,
        string? tecnico,
        string? especialidad,
        string? cliente,
        string? documento,
        DateTime? fechaInicio,
        DateTime? fechaFin)
    {
        var sql = @"
            SELECT so.*
            FROM ServiceOrders so
            INNER JOIN Technicians t ON so.TechnicianId = t.Id
            INNER JOIN Clients c ON so.ClientId = c.Id
            WHERE 1=1";

        var parameters = new DynamicParameters();

        if (!string.IsNullOrEmpty(estado))
        {
            sql += " AND so.Status = @Estado";
            parameters.Add("Estado", estado);
        }

        if (!string.IsNullOrEmpty(tecnico))
        {
            sql += " AND t.FullName ILIKE @Tecnico";
            parameters.Add("Tecnico", $"%{tecnico}%");
        }

        if (!string.IsNullOrEmpty(especialidad))
        {
            sql += " AND t.Specialty ILIKE @Especialidad";
            parameters.Add("Especialidad", $"%{especialidad}%");
        }

        if (!string.IsNullOrEmpty(cliente))
        {
            sql += " AND c.FullName ILIKE @Cliente";
            parameters.Add("Cliente", $"%{cliente}%");
        }

        if (!string.IsNullOrEmpty(documento))
        {
            sql += " AND c.DocumentNumber = @Documento";
            parameters.Add("Documento", documento);
        }

        if (fechaInicio.HasValue)
        {
            sql += " AND so.CreatedAt >= @FechaInicio";
            parameters.Add("FechaInicio", fechaInicio.Value);
        }

        if (fechaFin.HasValue)
        {
            sql += " AND so.CreatedAt <= @FechaFin";
            parameters.Add("FechaFin", fechaFin.Value);
        }

        sql += " ORDER BY so.CreatedAt DESC";

        return await _db.QueryAsync<ServiceOrder>(sql, parameters);
    }

}
