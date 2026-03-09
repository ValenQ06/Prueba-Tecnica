namespace PruebaComerciantes.Application.DTOs
{
    public record LoginResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiracion { get; set; }
    }
}
