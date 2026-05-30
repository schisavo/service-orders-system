using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Repositories;

namespace ServiceOrdersSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido con JWT
    public class ClientsController : ControllerBase
    {
        private readonly ClientRepository _repository;

        public ClientsController(ClientRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var clients = await _repository.GetAll();
            return Ok(clients);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var client = await _repository.GetById(id);
            if (client == null) return NotFound();
            return Ok(client);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            if (string.IsNullOrWhiteSpace(client.FullName))
                return BadRequest("Nombre requerido");
            if (string.IsNullOrWhiteSpace(client.DocumentNumber))
                return BadRequest("Documento de identidad requerido");

            try
            {
                var id = await _repository.Create(client);
                client.Id = id;
                return CreatedAtAction(nameof(GetById), new { id }, client);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            client.Id = id;
            await _repository.Update(client);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _repository.Delete(id);
            return NoContent();
        }
    }
}
