using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaComerciantes.Domain.Entities
{
    [Table("Establecimiento")]
    public class Establecimiento
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(150)]
        public required string Nombre { get; set; }

        [Column("Ingresos", TypeName = "decimal(18,2)")]
        public required decimal Ingresos { get; set; }

        public required int NumeroEmpleados { get; set; }
      
        public required int IdComerciante { get; set; }

        public DateTime? FechaActualizacion { get; set; }

        public int? UsuarioActualizacion { get; set; }

        // Propiedades de navegación
        public virtual Comerciante Comerciante { get; set; } = null!;
        public virtual Usuario? UsuarioAudit { get; set; }
    }
}