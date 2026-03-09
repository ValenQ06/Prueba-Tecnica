using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaComerciantes.Application.Interfaces;

namespace PruebaComerciantes.Controllers
{
    [Authorize(Roles = "Administrador")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportesController : ControllerBase
    {
        private readonly IReporteService _service;

        public ReportesController(IReporteService service)
        {
            _service = service;
        }

        [HttpGet("comerciantes")]
        public async Task<IActionResult> DescargarReporte()
        {
            var file = await _service.GenerarReporteAsync();

            return File(
                file,
                "text/csv",
                $"reporte_comerciantes_{DateTime.Now:yyyyMMdd}.csv"
            );
        }
    }
}
