using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Application.DTOs;
using ServiceOrdersSystem.Application.Interfaces;
using ServiceOrdersSystem.Domain.Entities;

namespace ServiceOrdersSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TechniciansController : ControllerBase
{
    private readonly ITechnicianRepository _repository;
    private readonly ILogger<TechniciansController> _logger;

    public TechniciansController(ITechnicianRepository repository, ILogger<TechniciansController> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var technicians = await _repository.GetAllAsync();
        var dtos = technicians.Select(t => new TechnicianDto
        {
            Id = t.Id,
            FullName = t.FullName,
            Phone = t.Phone,
            Specialty = t.Specialty
        });

        return Ok(dtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var technician = await _repository.GetByIdAsync(id);
        if (technician == null) return NotFound(new { Message = "Technician not found" });

        var dto = new TechnicianDto
        {
            Id = technician.Id,
            FullName = technician.FullName,
            Phone = technician.Phone,
            Specialty = technician.Specialty
        };

        return Ok(dto);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] TechnicianDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var technician = new Technician
        {
            FullName = dto.FullName,
            Phone = dto.Phone,
            Specialty = dto.Specialty
        };

        var id = await _repository.CreateAsync(technician);
        dto.Id = id;

        _logger.LogInformation("Technician created with Id {Id}", id);

        return CreatedAtAction(nameof(GetById), new { id }, dto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] TechnicianDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var technician = new Technician
        {
            Id = id,
            FullName = dto.FullName,
            Phone = dto.Phone,
            Specialty = dto.Specialty
        };

        var updated = await _repository.UpdateAsync(technician);
        if (!updated) return NotFound(new { Message = "Technician not found" });

        _logger.LogInformation("Technician updated with Id {Id}", id);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _repository.DeleteAsync(id);
        if (!deleted) return NotFound(new { Message = "Technician not found" });

        _logger.LogInformation("Technician deleted with Id {Id}", id);

        return NoContent();
    }
}
