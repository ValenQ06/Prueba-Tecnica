using Microsoft.EntityFrameworkCore;

namespace PruebaComerciantes.Application.DTOs
{
    [Keyless]
    public record ReporteComercianteDto
    {
        public string NombreRazonSocial { get; set; }

        public string Municipio { get; set; }

        public string? Telefono { get; set; }

        public string? CorreoElectronico { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Estado { get; set; }

        public int CantidadEstablecimientos { get; set; }

        public decimal TotalIngresos { get; set; }

        public int CantidadEmpleados { get; set; }
    }
}
