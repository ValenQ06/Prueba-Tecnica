using PruebaComerciantes.Application.DTOs;

namespace PruebaComerciantes.Application.Interfaces
{
    public interface IMunicipioService
    {
        Task<List<MunicipioDto>> ObtenerMunicipios();
    }
}