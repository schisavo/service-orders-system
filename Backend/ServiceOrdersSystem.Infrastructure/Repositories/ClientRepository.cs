using Dapper;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Data;

namespace ServiceOrdersSystem.Infrastructure.Repositories
{
    public class ClientRepository
    {
        private readonly DapperContext _context;

        public ClientRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Client>("SELECT * FROM Clients");
        }

        public async Task<Client?> GetById(int id)
        {
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Client>(
                "SELECT * FROM Clients WHERE Id = @Id", new { Id = id });
        }

        public async Task<int> Create(Client client)
        {
            using var connection = _context.CreateConnection();

            // Validar DocumentoIdentidad único
            var exists = await connection.ExecuteScalarAsync<int>(
                "SELECT COUNT(*) FROM Clients WHERE DocumentNumber = @DocumentNumber",
                new { client.DocumentNumber });

            if (exists > 0)
                throw new Exception("Documento de identidad ya registrado");

            var sql = @"INSERT INTO Clients (FullName, DocumentNumber, Address, Phone)
                        VALUES (@FullName, @DocumentNumber, @Address, @Phone)
                        RETURNING Id;";
            return await connection.ExecuteScalarAsync<int>(sql, client);
        }

        public async Task Update(Client client)
        {
            using var connection = _context.CreateConnection();
            var sql = @"UPDATE Clients
                        SET FullName = @FullName, DocumentNumber = @DocumentNumber,
                            Address = @Address, Phone = @Phone
                        WHERE Id = @Id;";
            await connection.ExecuteAsync(sql, client);
        }

        public async Task Delete(int id)
        {
            using var connection = _context.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Clients WHERE Id = @Id", new { Id = id });
        }
    }
}
