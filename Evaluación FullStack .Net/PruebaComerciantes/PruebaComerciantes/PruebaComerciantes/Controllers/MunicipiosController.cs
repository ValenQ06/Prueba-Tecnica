using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;

namespace PruebaComerciantes.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // endpoint privado
    public class MunicipiosController : ControllerBase
    {
        private readonly IMunicipioService _municipioService;

        public MunicipiosController(IMunicipioService municipioService)
        {
            _municipioService = municipioService;
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerMunicipios()
        {
            var municipios = await _municipioService.ObtenerMunicipios();

            var response = new ApiResponse<List<MunicipioDto>>
            {
                Success = true,
                Message = "Municipios obtenidos correctamente",
                Data = municipios
            };

            return Ok(response);
        }
    }
}