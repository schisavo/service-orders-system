using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Application.DTOs;
using ServiceOrdersSystem.Application.Interfaces;
using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] // Protegido con JWT
public class ClientsController : ControllerBase
{
    private readonly IClientRepository _repository;
    private readonly ILogger<ClientsController> _logger;

    public ClientsController(IClientRepository repository, ILogger<ClientsController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _repository.GetAllAsync();
        var dtos = clients.Select(c => new ClientDto
        {
            Id = c.Id,
            FullName = c.FullName,
            DocumentNumber = c.DocumentNumber,
            Address = c.Address,
            Phone = c.Phone
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var client = await _repository.GetByIdAsync(id);
        if (client == null) return NotFound(new { Message = "Client not found" });

        var dto = new ClientDto
        {
            Id = client.Id,
            FullName = client.FullName,
            DocumentNumber = client.DocumentNumber,
            Address = client.Address,
            Phone = client.Phone
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ClientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var client = new Client
            {
                FullName = dto.FullName,
                DocumentNumber = dto.DocumentNumber,
                Address = dto.Address,
                Phone = dto.Phone
            };

            var id = await _repository.CreateAsync(client);
            dto.Id = id;

            _logger.LogInformation("Client created with Id {Id}", id);

            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating client");
            return BadRequest(new { Message = "Error creating client" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ClientDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        dto.Id = id;
        var client = new Client
        {
            Id = dto.Id,
            FullName = dto.FullName,
            DocumentNumber = dto.DocumentNumber,
            Address = dto.Address,
            Phone = dto.Phone
        };

        var updated = await _repository.UpdateAsync(client);
        if (!updated) return NotFound(new { Message = "Client not found" });

        _logger.LogInformation("Client updated with Id {Id}", id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound(new { Message = "Client not found" });

        _logger.LogInformation("Client deleted with Id {Id}", id);

        return NoContent();
    }
}
