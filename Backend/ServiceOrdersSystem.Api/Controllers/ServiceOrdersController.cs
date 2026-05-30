using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Application.DTOs;
using ServiceOrdersSystem.Application.Interfaces;
using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServiceOrdersController : ControllerBase
{
    private readonly IServiceOrderRepository _repository;
    private readonly ILogger<ServiceOrdersController> _logger;

    public ServiceOrdersController(IServiceOrderRepository repository, ILogger<ServiceOrdersController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _repository.GetAllAsync();
        var dtos = orders.Select(o => new ServiceOrderDto
        {
            Id = o.Id,
            CreatedAt = o.CreatedAt,
            Status = o.Status,
            Description = o.Description,
            TechnicianId = o.TechnicianId,
            ClientId = o.ClientId
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _repository.GetByIdAsync(id);
        if (order == null) return NotFound(new { Message = "Service order not found" });

        var dto = new ServiceOrderDto
        {
            Id = order.Id,
            CreatedAt = order.CreatedAt,
            Status = order.Status,
            Description = order.Description,
            TechnicianId = order.TechnicianId,
            ClientId = order.ClientId
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ServiceOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var order = new ServiceOrder
            {
                CreatedAt = DateTime.Now,
                Status = dto.Status,
                Description = dto.Description,
                TechnicianId = dto.TechnicianId,
                ClientId = dto.ClientId
            };

            var id = await _repository.CreateAsync(order);
            dto.Id = id;
            dto.CreatedAt = order.CreatedAt;

            _logger.LogInformation("Service order created with Id {Id}", id);

            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating service order");
            return BadRequest(new { Message = "Error creating service order" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] ServiceOrderDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var order = new ServiceOrder
        {
            Id = id,
            CreatedAt = dto.CreatedAt,
            Status = dto.Status,
            Description = dto.Description,
            TechnicianId = dto.TechnicianId,
            ClientId = dto.ClientId
        };

        var updated = await _repository.UpdateAsync(order);
        if (!updated) return NotFound(new { Message = "Service order not found" });

        _logger.LogInformation("Service order updated with Id {Id}", id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound(new { Message = "Service order not found" });

        _logger.LogInformation("Service order deleted with Id {Id}", id);

        return NoContent();
    }

    [HttpGet("filtro")]
    public async Task<IActionResult> Filter(
        [FromQuery] string? estado,
        [FromQuery] string? tecnico,
        [FromQuery] string? especialidad,
        [FromQuery] string? cliente,
        [FromQuery] string? documento,
        [FromQuery] DateTime? fechaInicio,
        [FromQuery] DateTime? fechaFin)
    {
        var result = await _repository.FilterOrders(
            estado, tecnico, especialidad, cliente, documento, fechaInicio, fechaFin
        );

        return Ok(result);
    }
}
