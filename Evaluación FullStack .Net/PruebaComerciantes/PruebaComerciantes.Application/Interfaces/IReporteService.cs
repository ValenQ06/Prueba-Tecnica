namespace PruebaComerciantes.Application.Interfaces
{
    public interface IReporteService
    {
        Task<byte[]> GenerarReporteAsync();
    }
}
