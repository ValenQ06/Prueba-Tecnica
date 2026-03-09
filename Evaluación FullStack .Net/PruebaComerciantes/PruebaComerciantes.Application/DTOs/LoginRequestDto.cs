namespace PruebaComerciantes.Application.DTOs
{
    public record LoginRequestDto
    {
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }
    }

}
