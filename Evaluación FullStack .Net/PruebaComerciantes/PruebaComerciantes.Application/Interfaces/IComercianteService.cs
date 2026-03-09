using PruebaComerciantes.Application.DTOs;

namespace PruebaComerciantes.Application.Interfaces
{
    public interface IComercianteService
    {
        Task<ApiResponse<PagedResult<ComercianteDto>>> GetPagedAsync(
            string? nombre,
            DateTime? fechaRegistro,
            bool? estado,
            int page,
            int pageSize);

        Task<ApiResponse<ComercianteDto>> GetByIdAsync(int id);

        Task<ApiResponse<bool>> CreateAsync(ComercianteDto dto, string usuario);

        Task<ApiResponse<bool>> UpdateAsync(int id, ComercianteDto dto, string usuario);

        Task<ApiResponse<bool>> DeleteAsync(int id);

        Task<ApiResponse<bool>> ChangeStatusAsync(int id, bool estado, string usuario);
    }
}
