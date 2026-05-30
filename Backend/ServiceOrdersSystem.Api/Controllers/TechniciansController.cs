using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Repositories;

namespace ServiceOrdersSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido con JWT
    public class TechniciansController : ControllerBase
    {
        private readonly TechnicianRepository _repository;

        public TechniciansController(TechnicianRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var technicians = await _repository.GetAll();
            return Ok(technicians);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var technician = await _repository.GetById(id);
            if (technician == null) return NotFound();
            return Ok(technician);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Technician technician)
        {
            if (string.IsNullOrWhiteSpace(technician.FullName))
                return BadRequest("Nombre requerido");
            if (string.IsNullOrWhiteSpace(technician.Specialty))
                return BadRequest("Especialidad requerida");
            if (string.IsNullOrWhiteSpace(technician.Phone))
                return BadRequest("Teléfono requerido");

            var id = await _repository.Create(technician);
            technician.Id = id;
            return CreatedAtAction(nameof(GetById), new { id }, technician);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Technician technician)
        {
            technician.Id = id;
            await _repository.Update(technician);
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
