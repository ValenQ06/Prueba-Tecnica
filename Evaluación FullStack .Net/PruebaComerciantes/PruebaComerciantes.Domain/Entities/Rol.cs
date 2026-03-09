using PruebaComerciantes.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Rol")]
public class Rol
{
    [Key]
    public int Id { get; set; }

    public string Nombre { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}