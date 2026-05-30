using System.Text;
using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Data;

namespace ServiceOrdersSystem.Infrastructure.Repositories
{
    public class ServiceOrderRepository
    {
        private readonly DapperContext _context;

        public ServiceOrderRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ServiceOrder>> GetAll()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<ServiceOrder>("SELECT * FROM ServiceOrders");
        }

        public async Task<ServiceOrder?> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<ServiceOrder>(
                "SELECT * FROM ServiceOrders WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> Create(ServiceOrder order)
        {
            using var connection = _context.CreateConnection();

            // Validar que el Técnico exista
            var technicianExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Technicians WHERE Id = @TechnicianId",
                new { order.TechnicianId });
            if (technicianExists == 0)
                throw new Exception("Técnico no existe");

            // Validar que el Cliente exista
            var clientExists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Clients WHERE Id = @ClientId",
                new { order.ClientId });
            if (clientExists == 0)
                throw new Exception("Cliente no existe");

            var sql = @"INSERT INTO ServiceOrders (CreatedAt, Status, Description, TechnicianId, ClientId)
                        VALUES (@CreatedAt, @Status, @Description, @TechnicianId, @ClientId)
                        RETURNING Id;";
            return await connection.ExecuteScalarAsync<int>(sql, order);
        }

        public async Task Update(ServiceOrder order)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE ServiceOrders
                        SET Status = @Status, Description = @Description,
                            TechnicianId = @TechnicianId, ClientId = @ClientId
                        WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, order);
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM ServiceOrders WHERE Id = @Id", new { Id = id });
        }

        // Filtered Orders Method
        public async Task<IEnumerable<dynamic>> FilterOrders(
            string? estado,
            string? tecnico,
            string? especialidad,
            string? cliente,
            string? documento,
            DateTime? fechaInicio,
            DateTime? fechaFin)
        {
            using var connection = _context.CreateConnection();

            var sql = new StringBuilder(@"
                SELECT 
                    o.*,
                    t.FullName AS TecnicoNombre,
                    c.FullName AS ClienteNombre
                FROM ServiceOrders o
                INNER JOIN Technicians t ON o.TechnicianId = t.Id
                INNER JOIN Clients c ON o.ClientId = c.Id
                WHERE 1=1
            ");

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(estado))
            {
                sql.Append(" AND o.Status = @Estado");
                parameters.Add("Estado", estado);
            }

            if (!string.IsNullOrWhiteSpace(tecnico))
            {
                sql.Append(" AND t.FullName ILIKE @Tecnico");
                parameters.Add("Tecnico", $"%{tecnico}%");
            }

            if (!string.IsNullOrWhiteSpace(especialidad))
            {
                sql.Append(" AND t.Specialty ILIKE @Especialidad");
                parameters.Add("Especialidad", $"%{especialidad}%");
            }

            if (!string.IsNullOrWhiteSpace(cliente))
            {
                sql.Append(" AND c.FullName ILIKE @Cliente");
                parameters.Add("Cliente", $"%{cliente}%");
            }

            if (!string.IsNullOrWhiteSpace(documento))
            {
                sql.Append(" AND c.DocumentNumber = @Documento");
                parameters.Add("Documento", documento);
            }

            if (fechaInicio.HasValue)
            {
                sql.Append(" AND o.CreatedAt >= @FechaInicio");
                parameters.Add("FechaInicio", fechaInicio.Value);
            }

            if (fechaFin.HasValue)
            {
                sql.Append(" AND o.CreatedAt <= @FechaFin");
                parameters.Add("FechaFin", fechaFin.Value);
            }

            return await connection.QueryAsync<dynamic>(sql.ToString(), parameters);
        }

    }
}
