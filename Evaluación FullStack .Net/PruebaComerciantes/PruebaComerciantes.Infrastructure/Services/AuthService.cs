using Microsoft.EntityFrameworkCore;
using PruebaComerciantes.Application.DTOs;
using PruebaComerciantes.Application.Interfaces;
using PruebaComerciantes.Infrastructure.Data;
using PruebaComerciantes.Infrastructure.Security;

namespace PruebaComerciantes.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtGenerator _jwtGenerator;

        public AuthService(ApplicationDbContext context, JwtGenerator jwtGenerator)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var usuario = await _context.Usuarios
                .Include(x => x.Rol)
                .FirstOrDefaultAsync(u =>
                    u.CorreoElectronico == request.CorreoElectronico &&
                    u.Contrasena == request.Contrasena);

            if (usuario == null)
                throw new Exception("Credenciales inválidas");

            var token = _jwtGenerator.GenerateToken(usuario);

            return new LoginResponseDto
            {
                Token = token,
                Expiracion = DateTime.UtcNow.AddHours(1)
            };
        }
    }
}