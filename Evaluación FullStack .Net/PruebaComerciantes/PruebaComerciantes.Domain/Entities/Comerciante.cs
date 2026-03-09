using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaComerciantes.Domain.Entities
{
    [Table("Comerciante")]
    public class Comerciante
    {
        [Key]
        public int Id { get; set; }
        public required string NombreRazonSocial { get; set; }
        public required int IdMunicipio { get; set; }
        public string? Telefono { get; set; }

        public string? CorreoElectronico { get; set; }

        public DateTime FechaRegistro { get; set; }

        public string Estado { get; set; }

        public DateTime FechaActualizacion { get; set; }

        public required int UsuarioActualizacion { get; set; }

        [ForeignKey("IdMunicipio")]
        public virtual Municipio Municipio { get; set; } = null!;
        public virtual Usuario? Usuario { get; set; }
        public virtual ICollection<Establecimiento> Establecimientos { get; set; } = new List<Establecimiento>();
    }
}
