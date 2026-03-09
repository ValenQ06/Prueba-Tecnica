using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PruebaComerciantes.Domain.Entities
{
    [Table("Municipio")]
    public class Municipio
    {
        [Key]
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public virtual ICollection<Comerciante> Comerciantes { get; set; } = new List<Comerciante>();
    }
}
