using Microsoft.EntityFrameworkCore;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;
using PruebaComerciantes.Infrastructure.Data;
using System.Text;

namespace PruebaComerciantes.Infrastructure.Services
{
    public class ReporteService : IReporteService
    {
        private readonly ApplicationDbContext _context;

        public ReporteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<byte[]> GenerarReporteAsync()
        {
            var data = await _context
                .Set<ReporteComercianteDto>()
                .FromSqlRaw("EXEC sp_ReporteComerciantesActivos")
                .ToListAsync();

            var builder = new StringBuilder();

            // encabezado
            builder.AppendLine("NombreRazonSocial|Municipio|Telefono|CorreoElectronico|FechaRegistro|Estado|CantidadEstablecimientos|TotalIngresos|CantidadEmpleados");

            foreach (var item in data)
            {
                builder.AppendLine(
                    $"{item.NombreRazonSocial}|{item.Municipio}|{item.Telefono}|{item.CorreoElectronico}|{item.FechaRegistro:yyyy-MM-dd}|{item.Estado}|{item.CantidadEstablecimientos}|{item.TotalIngresos}|{item.CantidadEmpleados}"
                );
            }

            return Encoding.UTF8.GetBytes(builder.ToString());
        }
    }
}
