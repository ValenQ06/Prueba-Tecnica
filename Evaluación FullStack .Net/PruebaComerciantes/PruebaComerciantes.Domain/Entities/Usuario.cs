using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaComerciantes.Domain.Entities
{
    [Table("Usuario")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        public required string Nombre { get; set; }

        public required string CorreoElectronico { get; set; }

        public required string Contrasena { get; set; }

        public required int IdRol { get; set; }

        [ForeignKey("IdRol")]
        public virtual Rol Rol { get; set; } = null!;
    }
}
