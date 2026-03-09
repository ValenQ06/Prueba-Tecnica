using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;
using PruebaComerciantes.Domain.Entities;
using PruebaComerciantes.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace PruebaComerciantes.Infrastructure.Services
{
    public class ComercianteService : IComercianteService
    {
        private readonly ApplicationDbContext _context;

        public ComercianteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PagedResult<ComercianteDto>>> GetPagedAsync(
            string? nombre,
            DateTime? fechaRegistro,
            bool? estado,
            int page,
            int pageSize)
        {
            var query = _context.Comerciantes
                .Include(x => x.Municipio)
                .AsQueryable();

            if (!string.IsNullOrEmpty(nombre))
                query = query.Where(x => x.NombreRazonSocial.Contains(nombre));

            if (fechaRegistro.HasValue)
                query = query.Where(x => x.FechaRegistro.Date == fechaRegistro.Value.Date);

            if (estado.HasValue)
                query = query.Where(x => x.Estado == (estado.Value ? "Activo" : "Inactivo"));

            var total = await query.CountAsync();

            var data = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new ComercianteDto
                {
                    Id = x.Id,
                    NombreRazonSocial = x.NombreRazonSocial,
                    Municipio = x.Municipio.Nombre,
                    IdMunicipio = x.Municipio.Id,
                    Telefono = x.Telefono,
                    CorreoElectronico = x.CorreoElectronico,
                    FechaRegistro = x.FechaRegistro,
                    Estado = x.Estado
                })
                .ToListAsync();

            return new ApiResponse<PagedResult<ComercianteDto>>
            {
                Success = true,
                Message = "Consulta paginada obtenida correctamente",
                Data = new PagedResult<ComercianteDto>
                {
                    Page = page,
                    PageSize = pageSize,
                    TotalRecords = total,
                    Data = data
                }
            };
        }

        public async Task<ApiResponse<ComercianteDto>> GetByIdAsync(int id)
        {
            var comerciante = await _context.Comerciantes
                .Include(x => x.Municipio)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (comerciante == null)
                return new ApiResponse<ComercianteDto>
                {
                    Success = false,
                    Message = "Comerciante no encontrado"
                };

            var dto = new ComercianteDto
            {
                Id = comerciante.Id,
                NombreRazonSocial = comerciante.NombreRazonSocial,
                Municipio = comerciante.Municipio.Nombre,
                IdMunicipio = comerciante.Municipio.Id,
                Telefono = comerciante.Telefono,
                CorreoElectronico = comerciante.CorreoElectronico,
                FechaRegistro = comerciante.FechaRegistro,
                Estado = comerciante.Estado
            };

            return new ApiResponse<ComercianteDto>
            {
                Success = true,
                Message = "Comerciante encontrado",
                Data = dto
            };
        }

        public async Task<ApiResponse<bool>> CreateAsync(ComercianteDto dto, string usuario)
        {
            var usuarioDb = await _context.Usuarios
                            .FirstOrDefaultAsync(x => x.CorreoElectronico == usuario);
            var entity = new Comerciante
            {
                NombreRazonSocial = dto.NombreRazonSocial,
                IdMunicipio = dto.IdMunicipio,
                Telefono = dto.Telefono,
                CorreoElectronico = dto.CorreoElectronico,
                FechaRegistro = dto.FechaRegistro,
                Estado = dto.Estado,
                FechaActualizacion = DateTime.Now,
                UsuarioActualizacion = usuarioDb.Id
            };

            _context.Comerciantes.Add(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Comerciante creado correctamente",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> UpdateAsync(int id, ComercianteDto dto, string usuario)
        {
            var usuarioDb = await _context.Usuarios
                            .FirstOrDefaultAsync(x => x.CorreoElectronico == usuario);
            var entity = await _context.Comerciantes.FindAsync(id);

            if (entity == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Comerciante no encontrado"
                };

            entity.NombreRazonSocial = dto.NombreRazonSocial;
            entity.IdMunicipio = dto.IdMunicipio;
            entity.Telefono = dto.Telefono;
            entity.CorreoElectronico = dto.CorreoElectronico;
            entity.FechaRegistro = dto.FechaRegistro;
            entity.Estado = dto.Estado;

            entity.FechaActualizacion = DateTime.Now;
            entity.UsuarioActualizacion = usuarioDb.Id;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Comerciante actualizado correctamente",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> DeleteAsync(int id)
        {
            var entity = await _context.Comerciantes.FindAsync(id);

            if (entity == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Comerciante no encontrado"
                };

            _context.Comerciantes.Remove(entity);
            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Comerciante eliminado",
                Data = true
            };
        }

        public async Task<ApiResponse<bool>> ChangeStatusAsync(int id, bool estado, string usuario)
        {
            var entity = await _context.Comerciantes.FindAsync(id);

            if (entity == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Comerciante no encontrado"
                };

            // Buscar el usuario en la BD
            var usuarioDb = await _context.Usuarios
                .FirstOrDefaultAsync(x => x.CorreoElectronico == usuario);

            if (usuarioDb == null)
                return new ApiResponse<bool>
                {
                    Success = false,
                    Message = "Usuario no encontrado"
                };

            entity.Estado = estado ? "Activo" : "Inactivo";
            entity.UsuarioActualizacion = usuarioDb.Id;
            entity.FechaActualizacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ApiResponse<bool>
            {
                Success = true,
                Message = "Estado actualizado",
                Data = true
            };
        }
    }
}
