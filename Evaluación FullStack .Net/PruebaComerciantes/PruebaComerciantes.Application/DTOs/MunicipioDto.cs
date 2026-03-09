namespace PruebaComerciantes.Application.DTOs
{
    public record MunicipioDto
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
    }
}
