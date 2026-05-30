using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Data;

namespace ServiceOrdersSystem.Infrastructure.Repositories
{
    public class TechnicianRepository
    {
        private readonly DapperContext _context;

        public TechnicianRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Technician>> GetAll()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Technician>("SELECT * FROM Technicians");
        }

        public async Task<Technician?> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Technician>(
                "SELECT * FROM Technicians WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> Create(Technician technician)
        {
            using var connection = _context.CreateConnection();
            var sql = @"INSERT INTO Technicians (FullName, Phone, Specialty)
                        VALUES (@FullName, @Phone, @Specialty)
                        RETURNING Id;";
            return await connection.ExecuteScalarAsync<int>(sql, technician);
        }

        public async Task Update(Technician technician)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Technicians
                        SET FullName = @FullName, Phone = @Phone, Specialty = @Specialty
                        WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, technician);
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Technicians WHERE Id = @Id", new { Id = id });
        }
    }
}
