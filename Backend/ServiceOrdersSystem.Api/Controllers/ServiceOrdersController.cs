using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceOrdersSystem.Domain.Entities;
using ServiceOrdersSystem.Infrastructure.Repositories;

namespace ServiceOrdersSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protegido con JWT
    public class ServiceOrdersController : ControllerBase
    {
        private readonly ServiceOrderRepository _repository;

        public ServiceOrdersController(ServiceOrderRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _repository.GetAll();
            return Ok(orders);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _repository.GetById(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ServiceOrder order)
        {
            order.CreatedAt = DateTime.Now;

            try
            {
                var id = await _repository.Create(order);
                order.Id = id;
                return CreatedAtAction(nameof(GetById), new { id }, order);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] ServiceOrder order)
        {
            order.Id = id;
            await _repository.Update(order);
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
