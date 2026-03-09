
using System.ComponentModel.DataAnnotations;

namespace PruebaComerciantes.Application.DTOs
{
    public record ComercianteDto
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string NombreRazonSocial { get; set; }

        public int IdMunicipio { get; set; }

        public string? Telefono { get; set; }

        [EmailAddress]
        public string? CorreoElectronico { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string? Estado { get; set; }

        public string Municipio { get; set; }
    }
}
