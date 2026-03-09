using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;
using PruebaComerciantes.Infrastructure.Data;

namespace PruebaComerciantes.Infrastructure.Services
{
    public class MunicipioService : IMunicipioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;

        public MunicipioService(ApplicationDbContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }

        public async Task<List<MunicipioDto>> ObtenerMunicipios()
        {
            const string cacheKey = "municipios";

            if (!_cache.TryGetValue(cacheKey, out List<MunicipioDto> municipios))
            {
                municipios = await _context.Municipios
                    .Select(m => new MunicipioDto
                    {
                        Id = m.Id,
                        Nombre = m.Nombre
                    })
                    .ToListAsync();

                _cache.Set(cacheKey, municipios, TimeSpan.FromMinutes(30));
            }

            return municipios;
        }
    }
}