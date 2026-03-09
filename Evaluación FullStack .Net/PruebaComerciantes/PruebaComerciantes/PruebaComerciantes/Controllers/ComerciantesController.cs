using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;

namespace PruebaComerciantes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ComerciantesController : ControllerBase
    {
        private readonly IComercianteService _service;

        public ComerciantesController(IComercianteService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged(
            string? nombre,
            DateTime? fechaRegistro,
            bool? estado,
            int page = 1,
            int pageSize = 5)
        {
            var result = await _service.GetPagedAsync(nombre, fechaRegistro, estado, page, pageSize);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await _service.GetByIdAsync(id));
        }

        [HttpPost]
        public async Task<IActionResult> Create(ComercianteDto dto)
        {
            var usuario = User.Identity?.Name ?? "system";

            return Ok(await _service.CreateAsync(dto, usuario));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ComercianteDto dto)
        {
            var usuario = User.Identity?.Name ?? "system";

            return Ok(await _service.UpdateAsync(id, dto, usuario));
        }

        [Authorize(Roles = "Administrador")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _service.DeleteAsync(id));
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> ChangeStatus(int id, bool estado)
        {
            var usuario = User.Identity?.Name ?? "system";

            return Ok(await _service.ChangeStatusAsync(id, estado, usuario));
        }
    }
}